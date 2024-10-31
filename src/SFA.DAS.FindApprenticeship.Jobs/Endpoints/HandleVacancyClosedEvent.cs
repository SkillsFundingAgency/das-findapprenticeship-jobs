using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
using SFA.DAS.NServiceBus.AzureFunction.Attributes;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using System.Threading.Tasks;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class HandleVacancyClosedEvent
    {
        private readonly IVacancyClosedHandler _vacancyClosedHandler;

        public HandleVacancyClosedEvent(IVacancyClosedHandler vacancyClosedHandler)
        {
            _vacancyClosedHandler = vacancyClosedHandler;
        }

        [FunctionName("HandleVacancyClosedEvent")]
        public async Task Run([NServiceBusTrigger(Endpoint = QueueNames.VacancyClosed)] VacancyClosedEvent command, ILogger log)
        {
            log.LogInformation($"NServiceBus VacancyClosed trigger function executed at {DateTime.UtcNow}");
            await _vacancyClosedHandler.Handle(command, log);
            log.LogInformation($"NServiceBus VacancyClosed trigger function finished at {DateTime.UtcNow}");
        }
    }
}
