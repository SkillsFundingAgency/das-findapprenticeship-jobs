using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints;

public class IndexCleanupTimerTrigger(IIndexCleanupJobHandler handler)
{
    [Function("IndexCleanupTimerTrigger")]
    public async Task Run([TimerTrigger("0 */60 * * * *")] TimerInfo myTimer, ILogger log)
    {
        log.LogInformation($"IndexCleanupTimerTrigger function executed at: {DateTime.UtcNow}");
        await handler.Handle(log);
    }
}