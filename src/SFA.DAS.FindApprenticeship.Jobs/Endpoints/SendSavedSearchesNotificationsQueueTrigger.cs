using System.Text.Json;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Models;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints;

public class SendSavedSearchesNotificationsQueueTrigger(
    ISendSavedSearchesNotificationHandler handler,
    ILogger<SendSavedSearchesNotificationsQueueTrigger> logger)
{
    [Function("SendSavedSearchesNotificationsQueueTrigger")]
    public async Task Run([QueueTrigger(StorageQueueNames.SendSavedSearchNotificationAlert)] SavedCandidateSearchResultQueueItem queueItem)
    {
        logger.LogInformation("Send SavedSearchesNotificationsQueueTrigger executed at {DateTime}", DateTime.UtcNow);

        var savedSearch = JsonSerializer.Deserialize<SavedSearchCandidateVacancies>(queueItem.Payload);

        if (savedSearch != null) 
            await handler.Handle(savedSearch);
        else
            logger.LogInformation("Error occurred while deserializing SavedSearchQueueItem: {Item}", queueItem.Payload);
    }
}