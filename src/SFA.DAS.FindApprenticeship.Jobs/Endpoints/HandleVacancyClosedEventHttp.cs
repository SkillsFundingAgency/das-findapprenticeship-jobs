using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using System.Net.Http;
using System.Text.Json;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class HandleVacancyClosedEventHttp
    {
        private readonly IVacancyClosedHandler _vacancyClosedHandler;

        public HandleVacancyClosedEventHttp(IVacancyClosedHandler vacancyClosedHandler)
        {
            _vacancyClosedHandler = vacancyClosedHandler;
        }

        [FunctionName("HandleVacancyClosedEventHttp")]
        public async Task Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestMessage req, ILogger log)
        {
            log.LogInformation($"HandleVacancyClosedEvent HTTP trigger function executed at {DateTime.UtcNow}");

            var command = await JsonSerializer.DeserializeAsync<VacancyClosedEvent>(
                await req.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (command == null || command.VacancyId == Guid.Empty)
            {
                throw new ArgumentException(
                    $"HandleVacancyClosedEvent HTTP trigger function found empty request at {DateTime.UtcNow}",
                    nameof(req));
            }

            await _vacancyClosedHandler.Handle(command, log);
            log.LogInformation($"HandleVacancyClosedEvent HTTP trigger function finished at {DateTime.UtcNow}");
        }
    }
}
