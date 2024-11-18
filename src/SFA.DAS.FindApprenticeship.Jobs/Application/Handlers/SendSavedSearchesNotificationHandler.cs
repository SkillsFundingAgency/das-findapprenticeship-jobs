using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers
{
    public class SendSavedSearchesNotificationHandler(
        ILogger<SendApplicationReminderHandler> logger,
        IFindApprenticeshipJobsService findApprenticeshipJobsService,
        IBatchTaskRunner batchTaskRunner)
        : ISendSavedSearchesNotificationHandler
    {
        public async Task BatchHandle(List<SavedSearch> savedSearches)
        {
            // Add tasks to the runner
            for (var index = 1; index <= savedSearches.Count; index++)
            {
                var taskId = index;
                var savedSearchIndex = index;
                if (savedSearchIndex >= 0 && savedSearchIndex < savedSearches.Count)
                {
                    batchTaskRunner.AddTask(async () =>
                    {
                        logger.LogInformation("SendSavedSearchNotification Task {TaskId} started", taskId);
                        await findApprenticeshipJobsService.SendSavedSearchNotification(savedSearches[savedSearchIndex]);
                        logger.LogInformation("SendSavedSearchNotification Task {TaskId} completed", taskId);
                    });
                }
            }

            // Run tasks in batches
            await batchTaskRunner.RunBatchesAsync();
        }

        public async Task Handle(SavedSearch savedSearch)
        {
            await findApprenticeshipJobsService.SendSavedSearchNotification(savedSearch);
        }
    }
}
