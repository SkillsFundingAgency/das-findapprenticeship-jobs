using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints;

public class RecruitIndexerTimerTrigger(IRecruitIndexerJobHandler handler, ILogger<RecruitIndexerTimerTrigger> log)
{
    [Function("RecruitIndexerTimerTrigger")]
    public async Task Run([TimerTrigger("0 */30 * * * *")] TimerInfo myTimer)
    {
        if (DateTime.UtcNow.Hour >= 6 && DateTime.UtcNow.Hour <= 20)
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