using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
using SFA.DAS.NServiceBus.AzureFunction.Attributes;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class HandleVacancyApprovedEvent
    {
        private readonly IVacancyApprovedHandler _vacancyApprovedHandler;

        public HandleVacancyApprovedEvent(IVacancyApprovedHandler vacancyApprovedHandler)
        {
            _vacancyApprovedHandler = vacancyApprovedHandler;
        }

        [FunctionName("HandleVacancyApprovedEvent")]
        public async Task Run([NServiceBusTrigger(Endpoint = QueueNames.VacancyApproved)] VacancyApprovedEvent command, ILogger log)
        {
            log.LogInformation($"NServiceBus VacancyApproved trigger function executed at {DateTime.UtcNow}");
            await _vacancyApprovedHandler.Handle(command, log);
            log.LogInformation($"NServiceBus VacancyApproved trigger function finished at {DateTime.UtcNow}");
        }
    }
}
