using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using System;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class RecruitIndexerHttp
    {
        private readonly IRecruitIndexerJobHandler _handler;
        public RecruitIndexerHttp(IRecruitIndexerJobHandler handler)
        {
            _handler = handler;
        }

        [FunctionName("RecruitIndexerHttp")]
        public async Task Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req, ILogger log)
        {
            log.LogInformation($"Recruit Indexer HTTP function executed at: {DateTime.UtcNow}");
            await _handler.Handle();
        }
    }
}
