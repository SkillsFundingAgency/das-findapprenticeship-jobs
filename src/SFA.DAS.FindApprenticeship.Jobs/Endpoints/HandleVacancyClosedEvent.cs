using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class HandleVacancyClosedEvent(IVacancyClosedHandler vacancyClosedHandler, ILogger<HandleVacancyClosedEvent> log) : IHandleMessages<VacancyClosedEvent>
    {
        public async Task Handle(VacancyClosedEvent command,  IMessageHandlerContext context)
        {
            log.LogInformation($"NServiceBus VacancyClosed trigger function executed at {DateTime.UtcNow}");
            await vacancyClosedHandler.Handle(command);
            log.LogInformation($"NServiceBus VacancyClosed trigger function finished at {DateTime.UtcNow}");
        }
    }
}
