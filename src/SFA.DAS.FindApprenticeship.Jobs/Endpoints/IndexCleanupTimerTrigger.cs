using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints;

public class IndexCleanupTimerTrigger(IIndexCleanupJobHandler handler, ILogger<IndexCleanupTimerTrigger> log)
{
    [Function("IndexCleanupTimerTrigger")]
    public async Task Run([TimerTrigger("0 */60 * * * *")] TimerInfo myTimer)
    {
        log.LogInformation($"IndexCleanupTimerTrigger function executed at: {DateTime.UtcNow}");
        await handler.Handle();
    }
}