using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class RecruitIndexerTimerTrigger
    {
        private readonly IRecruitService _recruitService;
        private readonly IAzureSearchHelper _azureSearchHelperService;
        private const int PageNo = 1;
        private const int PageSize = 500;
        public RecruitIndexerTimerTrigger(IRecruitService recruitService, IAzureSearchHelper azureSearchHelper)
        {
            _recruitService = recruitService;
            _azureSearchHelperService = azureSearchHelper;
        }

        [FunctionName("RecruitIndexerTimerTrigger")]
        public async Task Run([TimerTrigger("0 */30 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Recruit Indexer function executed at: {DateTime.UtcNow}");
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
}
