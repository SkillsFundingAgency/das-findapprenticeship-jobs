using System;
using System.Collections.Generic;
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
        private readonly IAzureSearchIndexService _azureSearchIndexService;
        private const int PageNo = 1;
        private const int PageSize = 500;
        public RecruitIndexerTimerTrigger(IRecruitService recruitService, IAzureSearchIndexService azureSearchIndexService)
        {
            _recruitService = recruitService;   
            _azureSearchIndexService = azureSearchIndexService;
        }

        [FunctionName("RecruitIndexerTimerTrigger")]
        public async Task Run([TimerTrigger("0 */30 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Recruit Indexer function executed at: {DateTime.UtcNow}");
            var liveVacancies = await _recruitService.GetLiveVacancies(PageNo, PageSize);
            var hasData = liveVacancies.Vacancies.Any();
            if (hasData)
            {
                await _azureSearchIndexService.DeleteIndex("Apprenticeships");
                await _azureSearchIndexService.CreateIndex("Apprenticeships");
                var batchDocuments = liveVacancies.Vacancies.ToList().Select(a => (ApprenticeAzureSearchDocument)a).ToList();
                await _azureSearchIndexService.UploadDocuments(batchDocuments);

            }

            // to test azure search with the 'vacancies' index:
            //var document = liveVacancies.Vacancies.ToList()[0];
            //log.LogInformation($"Vacancy Id = {document.VacancyId} and VacancyTitle = {document.VacancyTitle}");
            //var vacanciesbatch = new List<ApprenticeAzureSearchDocument> { (ApprenticeAzureSearchDocument)document};
            //await _azureSearchIndexService.CreateIndex("vacancies");
            //await _azureSearchIndexService.UploadDocuments(vacanciesbatch);
        }
    }
}
