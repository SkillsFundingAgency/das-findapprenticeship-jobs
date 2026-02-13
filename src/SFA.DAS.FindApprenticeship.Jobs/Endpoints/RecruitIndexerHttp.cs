using Microsoft.AspNetCore.Http;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints;

public class RecruitIndexerHttp(IRecruitIndexerJobHandler handler,
    ILogger<RecruitIndexerHttp> log)
{
    [Function("RecruitIndexerHttp")]
    public async Task Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest _,
        CancellationToken cancellationToken)
    {
        try
        {
            log.LogInformation("Recruit Indexer HTTP function executed at: {UtcNow}", DateTime.UtcNow);
            await handler.Handle(cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}