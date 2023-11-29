using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class IndexCleanupTimerTrigger
    {
        private readonly IIndexCleanupJobHandler _handler;

        public IndexCleanupTimerTrigger(IIndexCleanupJobHandler handler)
        {
            _handler = handler;
        }

        [FunctionName("IndexCleanupTimerTrigger")]
        public async Task Run([TimerTrigger("* */60 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"IndexCleanupTimerTrigger function executed at: {DateTime.UtcNow}");
            await _handler.Handle(log);
        }
    }
}
