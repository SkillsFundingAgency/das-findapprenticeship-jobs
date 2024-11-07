using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using System.Text.Json;
using Esfa.Recruit.Vacancies.Client.Domain.Events;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints;

public class HandleVacancyUpdatedEventHttp(IVacancyUpdatedHandler vacancyUpdatedHandler, ILogger<HandleVacancyUpdatedEventHttp> log)
{
    [Function("HandleVacancyUpdatedEventHttp")]
    public async Task Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestMessage req)
    {
        log.LogInformation($"HandleVacancyUpdatedEvent HTTP trigger function executed at {DateTime.UtcNow}");

        var command = await JsonSerializer.DeserializeAsync<LiveVacancyUpdatedEvent>(
            await req.Content.ReadAsStreamAsync(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (command == null || command.VacancyId == Guid.Empty)
        {
            throw new ArgumentException(
                $"HandleVacancyUpdatedEvent HTTP trigger function found empty request at {DateTime.UtcNow}",
                nameof(req));
        }

        await vacancyUpdatedHandler.Handle(command);
        log.LogInformation($"HandleVacancyUpdatedEvent HTTP trigger function finished at {DateTime.UtcNow}");
    }
}