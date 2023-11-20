using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class RecruitIndexerTimerTrigger
    {
        private readonly IRecruitIndexerJobHandler _handler;

        public RecruitIndexerTimerTrigger(IRecruitIndexerJobHandler handler)
        {
            _handler = handler;
        }

        [FunctionName("RecruitIndexerTimerTrigger")]
        public async Task Run([TimerTrigger("0 */01 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Recruit Indexer function executed at: {DateTime.UtcNow}");
            await _handler.Handle();
        }
    }
}
