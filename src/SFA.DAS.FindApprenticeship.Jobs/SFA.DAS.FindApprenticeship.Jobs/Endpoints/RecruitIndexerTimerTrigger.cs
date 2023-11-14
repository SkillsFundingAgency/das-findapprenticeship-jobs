using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class RecruitIndexerTimerTrigger
    {
        [FunctionName("RecruitIndexerTimerTrigger")]
        public void Run([TimerTrigger("0 */30 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Recruit Indexer function executed at: {DateTime.UtcNow}");
        }
    }
}
