using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Models;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class UpdateInactiveCandidateAccountsQueueTrigger(
        IUpdateCandidateStatusHandler handler,
        ILogger<UpdateInactiveCandidateAccountsQueueTrigger> logger)
    {
        [Function(nameof(UpdateInactiveCandidateAccountsQueueTrigger))]
        public async Task Run([QueueTrigger(StorageQueueNames.UpdateInactiveCandidateAccountsDormant)] UpdateCandidateStatusQueueItem queueItem)
        {
            logger.LogInformation("UpdateInactiveCandidateAccountsQueueTrigger executed at {DateTime}", DateTime.UtcNow);
            await handler.BatchHandle(queueItem.Candidates);
        }
    }
}