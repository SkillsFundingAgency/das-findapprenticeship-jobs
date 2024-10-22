using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using System.Net.Http;
using System.Text.Json;
using Esfa.Recruit.Vacancies.Client.Domain.Events;
using Microsoft.Azure.Functions.Worker;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class HandleVacancyClosedEventHttp(IVacancyClosedHandler vacancyClosedHandler)
    {
        [Function("HandleVacancyClosedEventHttp")]
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

            await vacancyClosedHandler.Handle(command, log);
            log.LogInformation($"HandleVacancyClosedEvent HTTP trigger function finished at {DateTime.UtcNow}");
        }
    }
}
