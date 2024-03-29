using System;
using System.Threading.Tasks;
using Esfa.Recruit.Vacancies.Client.Domain.Events;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
using SFA.DAS.NServiceBus.AzureFunction.Attributes;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class HandleVacancyUpdatedEvent
    {
        private readonly IVacancyUpdatedHandler _vacancyUpdatedHandler;

        public HandleVacancyUpdatedEvent(IVacancyUpdatedHandler vacancyUpdatedHandler)
        {
            _vacancyUpdatedHandler = vacancyUpdatedHandler;
        }

        [FunctionName("HandleVacancyUpdatedEvent")]
        public async Task Run([NServiceBusTrigger(Endpoint = QueueNames.VacancyUpdated)]LiveVacancyUpdatedEvent message, ILogger log)
        {
            log.LogInformation($"NServiceBus VacancyUpdated trigger function executed at {DateTime.UtcNow}");
            await _vacancyUpdatedHandler.Handle(message, log);
            log.LogInformation($"NServiceBus VacancyUpdated trigger function finished at {DateTime.UtcNow}");

        }
    }
}
