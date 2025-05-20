using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints;

public class IndexCleanupTimerTrigger(IIndexCleanupJobHandler handler, ILogger<IndexCleanupTimerTrigger> log, IDateTimeService dateTimeService)
{
    [Function("IndexCleanupTimerTrigger")]
    public async Task Run([TimerTrigger("0 */60 * * * *")] TimerInfo myTimer)
    {
        if (dateTimeService.GetCurrentDateTime().Hour >= 6 && dateTimeService.GetCurrentDateTime().Hour <= 20)
        {
            log.LogInformation($"IndexCleanupTimerTrigger function executed at: {DateTime.UtcNow}");
            await handler.Handle();
        }
        else
        {
            log.LogInformation($"IndexCleanupTimerTrigger function skipping executing at: {DateTime.UtcNow}");
        }
    }
}