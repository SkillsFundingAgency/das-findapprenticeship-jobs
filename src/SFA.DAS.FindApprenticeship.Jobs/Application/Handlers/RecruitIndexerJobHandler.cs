using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
public class RecruitIndexerJobHandler : IRecruitIndexerJobHandler
{
    private readonly IRecruitService _recruitService;
    private readonly IAzureSearchHelper _azureSearchHelperService;
    private const int PageSize = 500;
    //private const string indexName = "apprenticeships";
    private const string indexName = "vacancies";

    public RecruitIndexerJobHandler(IRecruitService recruitService, IAzureSearchHelper azureSearchHelperService)
    {
        _recruitService = recruitService;
        _azureSearchHelperService = azureSearchHelperService;
    }

    public async Task Handle()
    {
        await _azureSearchHelperService.DeleteIndex(indexName);
        await _azureSearchHelperService.CreateIndex(indexName);

        var pageNo = 1;
        var totalPages = 100;

        while (pageNo <= totalPages)
        {
            var liveVacancies = await _recruitService.GetLiveVacancies(pageNo, PageSize);

            if (liveVacancies != null && liveVacancies.Vacancies.Any())
            {
                totalPages = liveVacancies.TotalPages;
                var batchDocuments = liveVacancies.Vacancies.Select(a => (ApprenticeAzureSearchDocument)a).ToList();
                await _azureSearchHelperService.UploadDocuments(batchDocuments);
                pageNo++;
            }
            else
            {
                break;
            }
        }
    }
}
