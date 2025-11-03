using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using System.Text.Json;
using Esfa.Recruit.Vacancies.Client.Domain.Events;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class HandleVacancyClosedEventHttp(IVacancyClosedHandler vacancyClosedHandler, ILogger<HandleVacancyClosedEventHttp> log)
    {
        [Function("HandleVacancyClosedEventHttp")]
        public async Task Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestMessage req)
        {
            log.LogInformation("HandleVacancyClosedEvent HTTP trigger function executed at {DateTime}", DateTime.UtcNow);

            var command = await JsonSerializer.DeserializeAsync<VacancyClosedEvent>(
                await req.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (command == null || command.VacancyId == Guid.Empty)
            {
                throw new ArgumentException(
                    $"HandleVacancyClosedEvent HTTP trigger function found empty request at {DateTime.UtcNow}",
                    nameof(req));
            }

            await vacancyClosedHandler.Handle(command);
            log.LogInformation("HandleVacancyClosedEvent HTTP trigger function finished at {DateTime}", DateTime.UtcNow);
        }
    }
}
