using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints;

public class RecruitIndexerTimerTrigger(IRecruitIndexerJobHandler handler)
{
    [Function("RecruitIndexerTimerTrigger")]
    public async Task Run([TimerTrigger("0 */30 * * * *")] TimerInfo myTimer, ILogger log)
    {
        log.LogInformation($"Recruit Indexer function executed at: {DateTime.UtcNow}");
        await handler.Handle();
    }
}