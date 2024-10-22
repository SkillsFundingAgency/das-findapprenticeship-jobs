using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints;

public class IndexCleanupHttp(IIndexCleanupJobHandler handler)
{
    [Function("IndexCleanupHttp")]
    public async Task Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req, ILogger log)
    {
        log.LogInformation($"Index Cleanup Http function executed at: {DateTime.UtcNow}");
        await handler.Handle(log);
    }
}