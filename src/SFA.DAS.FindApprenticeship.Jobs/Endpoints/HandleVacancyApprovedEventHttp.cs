using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using System.Net.Http;
using System.Text.Json;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class HandleVacancyApprovedEventHttp
    {
        private readonly IVacancyApprovedHandler _vacancyApprovedHandler;

        public HandleVacancyApprovedEventHttp(IVacancyApprovedHandler vacancyApprovedHandler)
        {
            _vacancyApprovedHandler = vacancyApprovedHandler;
        }

        [FunctionName("HandleVacancyApprovedEventHttp")]
        public async Task Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestMessage req, ILogger log)
        {
            log.LogInformation($"HandleVacancyApprovedEvent HTTP trigger function executed at {DateTime.UtcNow}");

            var command = await JsonSerializer.DeserializeAsync<VacancyApprovedEvent>(
                await req.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (command == null || command.VacancyId == Guid.Empty)
            {
                throw new ArgumentException(
                    $"HandleVacancyApprovedEvent HTTP trigger function found empty request at {DateTime.UtcNow}",
                    nameof(req));
            }

            await _vacancyApprovedHandler.Handle(command, log);
            log.LogInformation($"HandleVacancyApprovedEvent HTTP trigger function finished at {DateTime.UtcNow}");
        }
    }
}
