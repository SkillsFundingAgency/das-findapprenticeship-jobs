using Esfa.Recruit.Vacancies.Client.Domain.Events;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class HandleVacancyClosedEvent(IVacancyClosedHandler vacancyClosedHandler, ILogger<HandleVacancyClosedEvent> log) : IHandleMessages<VacancyClosedEvent>
    {
        public async Task Handle(VacancyClosedEvent command,  IMessageHandlerContext context)
        {
            log.LogInformation("NServiceBus VacancyClosed trigger function executed at {DateTime}", DateTime.UtcNow);
            await vacancyClosedHandler.Handle(command);
            log.LogInformation("NServiceBus VacancyClosed trigger function finished at {DateTime}", DateTime.UtcNow);
        }
    }
}
