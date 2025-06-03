using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints;

public class IndexCleanupTimerTrigger(IIndexCleanupJobHandler handler, ILogger<IndexCleanupTimerTrigger> log)
{
    [Function("IndexCleanupTimerTrigger")]
    public async Task Run([TimerTrigger("30 6-20/4 * * *")] TimerInfo myTimer)
    {
        log.LogInformation("IndexCleanupTimerTrigger function executed at: {When}", DateTime.UtcNow);
        await handler.Handle();
    }
}