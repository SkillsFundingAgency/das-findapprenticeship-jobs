using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;
using SFA.DAS.NServiceBus.AzureFunction.Attributes;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class HandleVacancyUpdatedEvent
    {
        [FunctionName("HandleVacancyUpdatedEvent")]
        public static async Task Run([NServiceBusTrigger(Endpoint = QueueNames.VacancyUpdated)]VacancyUpdatedEvent message, IVacancyUpdatedHandler handler, ILogger<VacancyUpdatedEvent> log)
        {
            log.LogInformation($"NServiceBus VacancyUpdated trigger function executed at {DateTime.Now}");
            await handler.Handle(message);
            log.LogInformation($"NServiceBus VacancyUpdated trigger function finished at {DateTime.Now}");

        }
    }
}
