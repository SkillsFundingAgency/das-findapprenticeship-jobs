using System;
using System.Threading.Tasks;
using Esfa.Recruit.Vacancies.Client.Domain.Events;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using NServiceBus;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class HandleVacancyApprovedEvent(IVacancyApprovedHandler vacancyApprovedHandler, ILogger log) : IHandleMessages<VacancyApprovedEvent>
    {
        public async Task Handle(VacancyApprovedEvent vacancyApprovedEvent, IMessageHandlerContext context)
        {
            log.LogInformation($"NServiceBus VacancyApproved trigger function executed at {DateTime.UtcNow}");
            await vacancyApprovedHandler.Handle(vacancyApprovedEvent, log);
            log.LogInformation($"NServiceBus VacancyApproved trigger function finished at {DateTime.UtcNow}");
        }
    }
}
