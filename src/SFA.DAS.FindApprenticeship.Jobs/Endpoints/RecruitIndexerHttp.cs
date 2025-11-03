using Microsoft.AspNetCore.Http;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints;

public class RecruitIndexerHttp(IRecruitIndexerJobHandler handler, ILogger<RecruitIndexerHttp> log)
{
    [Function("RecruitIndexerHttp")]
    public async Task Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest _)
    {
        log.LogInformation("Recruit Indexer HTTP function executed at: {DateTime}", DateTime.UtcNow);
        await handler.Handle();
    }
}