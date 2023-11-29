using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
using SFA.DAS.NServiceBus.AzureFunction.Attributes;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class HandleVacancyDeletedEvent
    {
        private readonly IVacancyDeletedHandler _vacancyDeletedHandler;

        public HandleVacancyDeletedEvent(IVacancyDeletedHandler vacancyDeletedHandler)
        {
            _vacancyDeletedHandler = vacancyDeletedHandler;
        }

        [FunctionName("HandleVacancyDeletedEvent")]
        public async Task Run([NServiceBusTrigger(Endpoint = QueueNames.VacancyDeleted)]VacancyDeletedEvent command, ILogger log)
        {
            log.LogInformation($"NServiceBus VacancyDeleted trigger function executed at {DateTime.Now}");
            await _vacancyDeletedHandler.Handle(command);
            log.LogInformation($"NServiceBus VacancyDeleted trigger function finished at {DateTime.Now}");
        }
    }
}
