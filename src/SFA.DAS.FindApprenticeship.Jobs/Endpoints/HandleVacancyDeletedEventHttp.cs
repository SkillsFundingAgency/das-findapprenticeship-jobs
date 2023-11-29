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
    public class HandleVacancyDeletedEventHttp
    {
        private readonly IVacancyDeletedHandler _vacancyDeletedHandler;

        public HandleVacancyDeletedEventHttp(IVacancyDeletedHandler vacancyDeletedHandler)
        {
            _vacancyDeletedHandler = vacancyDeletedHandler;
        }

        [FunctionName("HandleVacancyDeletedEventHttp")]
        public async Task Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestMessage req, ILogger log)
        {
            log.LogInformation($"HandleVacancyDeletedEvent HTTP trigger function executed at {DateTime.Now}");
            var command = JsonSerializer.Deserialize<VacancyDeletedEvent>(req.Content.ReadAsStream());
            await _vacancyDeletedHandler.Handle(command, log);
            log.LogInformation($"HandleVacancyDeletedEvent HTTP trigger function finished at {DateTime.Now}");
        }
    }
}
