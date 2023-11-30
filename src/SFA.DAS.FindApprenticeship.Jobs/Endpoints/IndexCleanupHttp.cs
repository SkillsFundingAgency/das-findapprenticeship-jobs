using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class IndexCleanupHttp
    {
        private readonly IIndexCleanupJobHandler _handler;

        public IndexCleanupHttp(IIndexCleanupJobHandler handler)
        {
            _handler = handler;
        }

        [FunctionName("IndexCleanupHttp")]
        public async Task Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req, ILogger log)
        {
            log.LogInformation($"Index Cleanup Http function executed at: {DateTime.UtcNow}");
            await _handler.Handle(log);
        }
    }
}
