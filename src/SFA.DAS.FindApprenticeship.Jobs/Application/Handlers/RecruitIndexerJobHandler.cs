using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.FindApprenticeship.Jobs.Domain;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
public class RecruitIndexerJobHandler : IRecruitIndexerJobHandler
{
    private readonly IFindApprenticeshipJobsService _recruitService;
    private readonly IAzureSearchHelper _azureSearchHelperService;
    private readonly IDateTimeService _dateTimeService;
    private const int PageSize = 500;

    public RecruitIndexerJobHandler(IFindApprenticeshipJobsService recruitService, IAzureSearchHelper azureSearchHelperService, IDateTimeService dateTimeService)
    {
        _recruitService = recruitService;
        _azureSearchHelperService = azureSearchHelperService;
        _dateTimeService = dateTimeService;
    }

    public async Task Handle()
    {
        var indexName = $"{Constants.IndexPrefix}{_dateTimeService.GetCurrentDateTime().ToString(Constants.IndexDateSuffixFormat)}";

        await _azureSearchHelperService.CreateIndex(indexName);

        var pageNo = 1;
        var totalPages = 100;
        var updateAlias = false;
        var batchDocuments = new List<ApprenticeAzureSearchDocument>();

        while (pageNo <= totalPages)
        {
            var liveVacancies = await _recruitService.GetLiveVacancies(pageNo, PageSize);
            var nhsLiveVacancies = await _recruitService.GetNhsLiveVacancies();

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

                await _azureSearchHelperService.UploadDocuments(indexName, batchDocuments);
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
            await _azureSearchHelperService.UpdateAlias(Constants.AliasName, indexName);
        }
    }
}