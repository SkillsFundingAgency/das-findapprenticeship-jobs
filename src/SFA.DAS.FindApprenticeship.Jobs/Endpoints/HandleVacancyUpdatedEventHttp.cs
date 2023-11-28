using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class HandleVacancyUpdatedEventHttp
    {
        private readonly IVacancyUpdatedHandler _vacancyUpdatedHandler;

        public HandleVacancyUpdatedEventHttp(IVacancyUpdatedHandler vacancyUpdatedHandler)
        {
            _vacancyUpdatedHandler = vacancyUpdatedHandler;
        }

        [FunctionName("HandleVacancyUpdatedEventHttp")]
        public async Task Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req, VacancyUpdatedEvent message, ILogger log)
        {
            log.LogInformation($"HandleVacancyUpdatedEvent HTTP trigger function executed at {DateTime.Now}");
            await _vacancyUpdatedHandler.Handle(message);
            log.LogInformation($"HandleVacancyUpdatedEvent HTTP trigger function finished at {DateTime.Now}");
        }
    }
}
