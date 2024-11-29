using Esfa.Recruit.Vacancies.Client.Domain.Events;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class HandleVacancyApprovedEvent(IVacancyApprovedHandler vacancyApprovedHandler, ILogger<HandleVacancyApprovedEvent> log) : IHandleMessages<VacancyApprovedEvent>
    {
        public async Task Handle(VacancyApprovedEvent vacancyApprovedEvent, IMessageHandlerContext context)
        {
            log.LogInformation($"NServiceBus VacancyApproved trigger function executed at {DateTime.UtcNow}");
            await vacancyApprovedHandler.Handle(vacancyApprovedEvent);
            log.LogInformation($"NServiceBus VacancyApproved trigger function finished at {DateTime.UtcNow}");
        }
    }
}
