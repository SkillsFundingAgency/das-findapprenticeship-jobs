using Microsoft.AspNetCore.Http;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Models;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class UpdateInactiveCandidateAccountsHttpTrigger(
        IGetDormantCandidateAccountsHandler handler,
        ILogger<UpdateInactiveCandidateAccountsHttpTrigger> logger)
    {
        [QueueOutput(StorageQueueNames.UpdateInactiveCandidateAccountsDormant)]
        [Function(nameof(UpdateInactiveCandidateAccountsHttpTrigger))]
        public async Task<UpdateCandidateStatusQueueItem> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            logger.LogInformation("UpdateInactiveCandidateAccountsHttpTrigger HTTP function executed at: {DateTime}", DateTime.UtcNow);

            var candidates = await handler.Handle();

            var unsubscribeQueueItem = new UpdateCandidateStatusQueueItem();

            foreach (var candidate in candidates)
            {
                unsubscribeQueueItem.Candidates.Add(candidate);
            }

            return unsubscribeQueueItem;
        }
    }
}