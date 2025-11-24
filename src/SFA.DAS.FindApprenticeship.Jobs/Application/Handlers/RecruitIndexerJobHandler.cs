using SFA.DAS.FindApprenticeship.Jobs.Domain;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Alerting;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;

public class RecruitIndexerJobHandler(
    IFindApprenticeshipJobsService findApprenticeshipJobsService,
    IAzureSearchHelper azureSearchHelperService,
    IDateTimeService dateTimeService,
    IApprenticeAzureSearchDocumentFactory recruitDocumentFactory,
    IIndexingAlertsManager indexingAlertsManager)
    : IRecruitIndexerJobHandler
{
    private const int PageSize = 500;

    public async Task Handle(CancellationToken cancellationToken = default)
    {
        var oldStats = await azureSearchHelperService.GetAliasStatisticsAsync(Constants.AliasName, cancellationToken);
        
        var indexName = $"{Constants.IndexPrefix}{dateTimeService.GetCurrentDateTime().ToString(Constants.IndexDateSuffixFormat)}";
        await azureSearchHelperService.CreateIndex(indexName);

        var pageNo = 1;
        var totalPages = 100;
        var updateAlias = false;
        while (pageNo <= totalPages)
        {
            var liveVacancies = await findApprenticeshipJobsService.GetLiveVacancies(pageNo, PageSize);
            totalPages = liveVacancies?.TotalPages ?? 0;

            var vacancies = liveVacancies?.Vacancies.ToList() ?? [];
            if (vacancies is not { Count: > 0 })
            {
                break;
            }

            var documents = vacancies
                .SelectMany(recruitDocumentFactory.Create)
                .ToList();

            await azureSearchHelperService.UploadDocuments(indexName, documents);
            pageNo++;
            updateAlias = true;
        }

        // Retrieve NHS live vacancies
        var nhsLiveVacancies = await findApprenticeshipJobsService.GetNhsLiveVacancies();
        if (nhsLiveVacancies != null && nhsLiveVacancies.Vacancies.Any())
        {
            var documents = nhsLiveVacancies.Vacancies
                .Where(fil => string.Equals(fil.Address?.Country, Constants.EnglandOnly, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => (ApprenticeAzureSearchDocument)x)
                .ToList();

            if (documents is { Count: 0 })
            {
                await indexingAlertsManager.SendNhsImportAlertAsync(cancellationToken);
            }
            
            await azureSearchHelperService.UploadDocuments(indexName, documents);
            updateAlias = true;
        }
        else
        {
            await indexingAlertsManager.SendNhsApiAlertAsync(cancellationToken);
        }

        // Retrieve Civil Service live vacancies
        var civilServiceLiveVacancies = await findApprenticeshipJobsService.GetCivilServiceLiveVacancies();
        if (civilServiceLiveVacancies != null && civilServiceLiveVacancies.Vacancies.Any())
        {
            var documents = civilServiceLiveVacancies.Vacancies
                .Where(fil => string.Equals(fil.Address.Country, Constants.EnglandOnly, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => (ApprenticeAzureSearchDocument)x)
                .ToList();

            if (documents is { Count: 0 })
            {
                await indexingAlertsManager.SendCsjImportAlertAsync(cancellationToken);
            }

            await azureSearchHelperService.UploadDocuments(indexName, documents);
            updateAlias = true;
        }

        if (updateAlias)
        {
            await azureSearchHelperService.UpdateAlias(Constants.AliasName, indexName);
            var newStats = await azureSearchHelperService.GetAliasStatisticsAsync(Constants.AliasName, cancellationToken);
            await indexingAlertsManager.VerifySnapshotsAsync(oldStats, newStats, cancellationToken);
        }
    }
}