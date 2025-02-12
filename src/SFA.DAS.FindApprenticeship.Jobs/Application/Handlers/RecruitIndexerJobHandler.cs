using SFA.DAS.FindApprenticeship.Jobs.Application.Extensions;
using SFA.DAS.FindApprenticeship.Jobs.Domain;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;

public class RecruitIndexerJobHandler(
    IFindApprenticeshipJobsService findApprenticeshipJobsService,
    IAzureSearchHelper azureSearchHelperService,
    IDateTimeService dateTimeService)
    : IRecruitIndexerJobHandler
{
    private const int PageSize = 500; 
    

    public async Task Handle()
    {
        var indexName = $"{Constants.IndexPrefix}{dateTimeService.GetCurrentDateTime().ToString(Constants.IndexDateSuffixFormat)}";

        await azureSearchHelperService.CreateIndex(indexName);

        var pageNo = 1;
        var totalPages = 100;
        var updateAlias = false;
        var batchDocuments = new List<ApprenticeAzureSearchDocument>();

        while (pageNo <= totalPages)
        {
            var liveVacancies = await findApprenticeshipJobsService.GetLiveVacancies(pageNo, PageSize);
            var nhsLiveVacancies = await findApprenticeshipJobsService.GetNhsLiveVacancies();

            totalPages = Math.Max(liveVacancies?.TotalPages ?? 0, nhsLiveVacancies?.TotalPages ?? 0);

            if (liveVacancies != null || nhsLiveVacancies != null)
            {
                if (liveVacancies != null && liveVacancies.Vacancies.Any())
                {
                    var documents = liveVacancies.Vacancies.SelectMany(ApprenticeAzureSearchDocumentFactory.Create);
                    batchDocuments.AddRange(documents);
                }
                    
                if (nhsLiveVacancies != null && nhsLiveVacancies.Vacancies.Any())
                {
                    batchDocuments.AddRange(nhsLiveVacancies.Vacancies
                        .Where(fil => string.Equals(fil.Address?.Country, Constants.EnglandOnly, StringComparison.InvariantCultureIgnoreCase))
                        .Select(vacancy => (ApprenticeAzureSearchDocument) vacancy));
                }

                await azureSearchHelperService.UploadDocuments(indexName, batchDocuments);
                pageNo++;
                updateAlias = true;
            }
            else
            {
                break;
            }
        }

        if (updateAlias)
        {
            await azureSearchHelperService.UpdateAlias(Constants.AliasName, indexName);
        }
    }
}