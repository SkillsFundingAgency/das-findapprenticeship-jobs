using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;
using System.Net.Http;
using System.Text.Json;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class HandleVacancyUpdatedEventHttp
    {
        private readonly IVacancyUpdatedHandler _vacancyUpdatedHandler;

        public HandleVacancyUpdatedEventHttp(IVacancyUpdatedHandler vacancyUpdatedHandler)
        {
            _vacancyUpdatedHandler = vacancyUpdatedHandler;
        }

        [FunctionName("HandleVacancyUpdatedEventHttp")]
        public async Task Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestMessage req, ILogger log)
        {
            log.LogInformation($"HandleVacancyUpdatedEvent HTTP trigger function executed at {DateTime.UtcNow}");
            var message = JsonSerializer.Deserialize<VacancyUpdatedEvent>(req.Content.ReadAsStream());

            if (message == null || message.VacancyReference == null)
            {
                log.LogInformation($"HandleVacancyUpdatedEvent HTTP trigger function found empty request at {DateTime.UtcNow}");
            }
            else
            {
                await _vacancyUpdatedHandler.Handle(message, log);
                log.LogInformation($"HandleVacancyUpdatedEvent HTTP trigger function finished at {DateTime.UtcNow}");
            }
        }
    }
}
