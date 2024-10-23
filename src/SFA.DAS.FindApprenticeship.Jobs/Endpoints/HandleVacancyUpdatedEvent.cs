using Esfa.Recruit.Vacancies.Client.Domain.Events;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class HandleVacancyUpdatedEvent(IVacancyUpdatedHandler vacancyUpdatedHandler,ILogger<HandleVacancyUpdatedEvent> log): IHandleMessages<LiveVacancyUpdatedEvent>
    {
        public async Task Handle(LiveVacancyUpdatedEvent message, IMessageHandlerContext context)
        {
            log.LogInformation($"NServiceBus VacancyUpdated trigger function executed at {DateTime.UtcNow}");
            await vacancyUpdatedHandler.Handle(message);
            log.LogInformation($"NServiceBus VacancyUpdated trigger function finished at {DateTime.UtcNow}");

        }
    }
}