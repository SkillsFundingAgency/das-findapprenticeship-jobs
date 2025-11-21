using SFA.DAS.FindApprenticeship.Jobs.Application.Services;
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

    public async Task Handle()
    {
        var oldStats = await azureSearchHelperService.GetAliasStatisticsAsync(Constants.AliasName);
        
        var indexName = $"{Constants.IndexPrefix}{dateTimeService.GetCurrentDateTime().ToString(Constants.IndexDateSuffixFormat)}";
        await azureSearchHelperService.CreateIndex(indexName);

        var pageNo = 1;
        var totalPages = 100;
        var updateAlias = false;
        long totalDocumentsCount = 0;
        while (pageNo <= totalPages)
        {
            var liveVacancies = await findApprenticeshipJobsService.GetLiveVacancies(pageNo, PageSize);
            totalPages = liveVacancies?.TotalPages ?? 0;

            var vacancies = liveVacancies?.Vacancies.ToList() ?? [];
            if (vacancies is not { Count: >0 })
            {
                break;
            }
            
            var documents = vacancies
                .SelectMany(recruitDocumentFactory.Create)
                .ToList();
            
            await azureSearchHelperService.UploadDocuments(indexName, documents);
            pageNo++;
            totalDocumentsCount += documents.Count;
            updateAlias = true;
        }
        
        var nhsLiveVacancies = await findApprenticeshipJobsService.GetNhsLiveVacancies();
        if (nhsLiveVacancies != null && nhsLiveVacancies.Vacancies.Any())
        {
            var documents = nhsLiveVacancies.Vacancies
                .Where(fil => string.Equals(fil.Address?.Country, Constants.EnglandOnly, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => (ApprenticeAzureSearchDocument)x)
                .ToList();

            if (documents is { Count: 0 })
            {
                await indexingAlertsManager.SendNhsImportAlertAsync();
            }
            
            await azureSearchHelperService.UploadDocuments(indexName, documents);
            totalDocumentsCount += documents.Count;
            updateAlias = true;
        }
        else
        {
            await indexingAlertsManager.SendNhsApiAlertAsync();
        }

        if (updateAlias)
        {
            await azureSearchHelperService.UpdateAlias(Constants.AliasName, indexName);
            var newStats = new IndexStatistics(totalDocumentsCount);
            await indexingAlertsManager.VerifySnapshotsAsync(oldStats, newStats);
        }
    }
}