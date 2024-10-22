using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using System;
using Microsoft.Azure.Functions.Worker;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints;

public class RecruitIndexerHttp(IRecruitIndexerJobHandler handler)
{
    [Function("RecruitIndexerHttp")]
    public async Task Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req, ILogger log)
    {
        log.LogInformation($"Recruit Indexer HTTP function executed at: {DateTime.UtcNow}");
        await handler.Handle();
    }
}