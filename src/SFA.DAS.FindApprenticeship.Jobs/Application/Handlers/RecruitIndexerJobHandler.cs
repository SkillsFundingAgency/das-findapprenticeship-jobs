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
                if (liveVacancies.Vacancies.Any())
                {
                    batchDocuments = liveVacancies.Vacancies.Select(a => (ApprenticeAzureSearchDocument)a).ToList();
                }

                if (nhsLiveVacancies.Vacancies.Any())
                {
                    foreach (var vacancy in nhsLiveVacancies.Vacancies)
                    {
                        batchDocuments.Add((ApprenticeAzureSearchDocument)vacancy);
                    }
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