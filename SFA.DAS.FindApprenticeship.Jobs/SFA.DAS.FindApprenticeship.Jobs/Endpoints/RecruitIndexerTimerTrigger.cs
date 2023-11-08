using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class RecruitIndexerTimerTrigger
    {
        private readonly IRecruitService _recruitService;
        private const int PageNo = 1;
        private const int PageSize = 500;
        public RecruitIndexerTimerTrigger(IRecruitService recruitService)
        {
            _recruitService = recruitService;   
        }

        [FunctionName("RecruitIndexerTimerTrigger")]
        public async Task Run([TimerTrigger("0 */30 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Recruit Indexer function executed at: {DateTime.UtcNow}");

            var liveVacancies = await _recruitService.GetLiveVacancies(PageNo, PageSize);

            // TODO - Create index
        }
    }
}
