using System;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using System.Threading.Tasks;
using Esfa.Recruit.Vacancies.Client.Domain.Events;
using Microsoft.Azure.Functions.Worker;
using NServiceBus;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class HandleVacancyClosedEvent(IVacancyClosedHandler vacancyClosedHandler, ILogger log) : IHandleMessages<VacancyClosedEvent>
    {
        [Function("HandleVacancyClosedEvent")]
        public async Task Handle(VacancyClosedEvent command,  IMessageHandlerContext context)
        {
            log.LogInformation($"NServiceBus VacancyClosed trigger function executed at {DateTime.UtcNow}");
            await vacancyClosedHandler.Handle(command, log);
            log.LogInformation($"NServiceBus VacancyClosed trigger function finished at {DateTime.UtcNow}");
        }
    }
}
