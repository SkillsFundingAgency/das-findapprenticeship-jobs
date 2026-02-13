using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints;

public class RecruitIndexerTimerTrigger(IRecruitIndexerJobHandler handler, ILogger<RecruitIndexerTimerTrigger> log)
{
    [Function("RecruitIndexerTimerTrigger")]
    public async Task Run([TimerTrigger("0 6-20/4 * * *")] TimerInfo myTimer)
    {
        log.LogInformation("Recruit Indexer function executed at: {When}", DateTime.UtcNow);
        await handler.Handle();
    }
}