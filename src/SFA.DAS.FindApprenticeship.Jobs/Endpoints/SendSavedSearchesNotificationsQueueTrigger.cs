using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Models;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints;

public class SendSavedSearchesNotificationsQueueTrigger(ISendSavedSearchesNotificationHandler handler)
{
    [Function("SendSavedSearchesNotificationsQueueTrigger")]
    public async Task Run([QueueTrigger(StorageQueueNames.SendSavedSearchNotificationAlert)] List<SavedSearchQueueItem> queueItem,
        ILogger<SendSavedSearchesNotificationsQueueTrigger> logger)
    {
        logger.LogInformation("Send SavedSearchesNotificationsQueueTrigger executed at {DateTime}", DateTime.UtcNow);
        await handler.Handle(queueItem.Select(item => (SavedSearch)item).ToList());
    }
}