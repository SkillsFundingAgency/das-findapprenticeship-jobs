using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.FindApprenticeship.Jobs.Domain;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
public class RecruitIndexerJobHandler : IRecruitIndexerJobHandler
{
    private readonly IRecruitService _recruitService;
    private readonly IAzureSearchHelper _azureSearchHelperService;
    private readonly IDateTimeService _dateTimeService;
    private const int PageSize = 500;

    public RecruitIndexerJobHandler(IRecruitService recruitService, IAzureSearchHelper azureSearchHelperService, IDateTimeService dateTimeService)
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

        while (pageNo <= totalPages)
        {
            var liveVacancies = await _recruitService.GetLiveVacancies(pageNo, PageSize);

            if (liveVacancies != null && liveVacancies.Vacancies.Any())
            {
                totalPages = liveVacancies.TotalPages;
                var batchDocuments = liveVacancies.Vacancies.Select(a => (ApprenticeAzureSearchDocument)a).ToList();
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
