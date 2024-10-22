using System;
using System.Threading.Tasks;
using Esfa.Recruit.Vacancies.Client.Domain.Events;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class HandleVacancyUpdatedEvent(IVacancyUpdatedHandler vacancyUpdatedHandler,ILogger log): IHandleMessages<LiveVacancyUpdatedEvent>
    {
        public async Task Handle(LiveVacancyUpdatedEvent message, IMessageHandlerContext context)
        {
            log.LogInformation($"NServiceBus VacancyUpdated trigger function executed at {DateTime.UtcNow}");
            await vacancyUpdatedHandler.Handle(message, log);
            log.LogInformation($"NServiceBus VacancyUpdated trigger function finished at {DateTime.UtcNow}");

        }
    }
}