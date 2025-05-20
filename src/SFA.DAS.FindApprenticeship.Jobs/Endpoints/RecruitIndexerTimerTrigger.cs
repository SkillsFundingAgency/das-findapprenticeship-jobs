using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints;

public class RecruitIndexerTimerTrigger(IRecruitIndexerJobHandler handler, ILogger<RecruitIndexerTimerTrigger> log, IDateTimeService dateTimeService)
{
    [Function("RecruitIndexerTimerTrigger")]
    public async Task Run([TimerTrigger("0 */30 * * * *")] TimerInfo myTimer)
    {
        if (dateTimeService.GetCurrentDateTime().Hour >= 6 && dateTimeService.GetCurrentDateTime().Hour <= 20)
        {
            log.LogInformation($"Recruit Indexer function executed at: {DateTime.UtcNow}");
            await handler.Handle();
        }
        else
        {
            log.LogInformation($"Recruit Indexer function skipping indexing at: {DateTime.UtcNow}");
        }
    }
}