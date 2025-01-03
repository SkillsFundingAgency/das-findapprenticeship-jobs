using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Models;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class UpdateInactiveCandidateAccountsTimerTrigger(
        IGetDormantCandidateAccountsHandler handler,
        ILogger<UpdateInactiveCandidateAccountsTimerTrigger> logger)
    {
        [QueueOutput(StorageQueueNames.UpdateInactiveCandidateAccountsDormant)]
        [Function(nameof(UpdateInactiveCandidateAccountsTimerTrigger))]
        public async Task<UpdateCandidateStatusQueueItem> Run([TimerTrigger("0 0 1 * * MON")] TimerInfo myTimerInfo)
        {
            logger.LogInformation("UnsubscribeSavedSearchTimerTrigger function executed at: {DateTime}", DateTime.UtcNow);

            var candidates = await handler.Handle();

            var updateCandidateStatusQueueItem = new UpdateCandidateStatusQueueItem();

            foreach (var candidate in candidates)
            {
                updateCandidateStatusQueueItem.Candidates.Add(candidate);
            }

            return updateCandidateStatusQueueItem;
        }
    }
}