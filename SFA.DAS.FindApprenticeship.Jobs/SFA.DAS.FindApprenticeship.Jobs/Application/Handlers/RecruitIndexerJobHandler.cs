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
    private const int PageNo = 1;
    private const int PageSize = 500;

    public RecruitIndexerJobHandler(IRecruitService recruitService, IAzureSearchHelper azureSearchHelperService)
    {
        _recruitService = recruitService;
        _azureSearchHelperService = azureSearchHelperService;
    }

    public async Task Handle()
    {
        var liveVacancies = await _recruitService.GetLiveVacancies(PageNo, PageSize);
        var hasData = liveVacancies != null && liveVacancies.Vacancies.Any();
        if (hasData)
        {
            await _azureSearchHelperService.DeleteIndex("apprenticeships");
            await _azureSearchHelperService.CreateIndex("apprenticeships");
            var batchDocuments = liveVacancies.Vacancies.ToList().Select(a => (ApprenticeAzureSearchDocument)a).ToList();
            await _azureSearchHelperService.UploadDocuments(batchDocuments);
        }

        // to test azure search with the 'vacancies' index:
        //    var document = liveVacancies.Vacancies.ToList()[0];
        //    log.LogInformation($"Vacancy Id = {document.VacancyId} and VacancyTitle = {document.VacancyTitle}");
        //    var vacanciesbatch = new List<ApprenticeAzureSearchDocument> { (ApprenticeAzureSearchDocument)document };
        //    await _azureSearchHelperService.CreateIndex("vacancies");
        //    await _azureSearchHelperService.UploadDocuments(vacanciesbatch);
    }
}
