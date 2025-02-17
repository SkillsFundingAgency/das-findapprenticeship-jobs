using System.Text.Json;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Models;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints;

public class GetSavedSearchesForCandidateQueueTrigger(
    IGetGetCandidateSavedSearchHandler handler,
    ILogger<SendSavedSearchesNotificationsQueueTrigger> logger)
{
    [Function("GetSavedSearchesForCandidateQueueTrigger")]
    [QueueOutput(StorageQueueNames.SendSavedSearchNotificationAlert)]
    public async Task<SavedCandidateSearchResultQueueItem?> Run([QueueTrigger(StorageQueueNames.GetSavedSearchNotifications)] SavedSearchQueueItem queueItem)
    {
        logger.LogInformation("Send GetSavedSearchesForCandidateQueueTrigger executed at {DateTime}", DateTime.UtcNow);

        var savedSearch = JsonSerializer.Deserialize<SavedSearchResult>(queueItem.Payload);

        if (savedSearch != null)
        {
            //Get Candidates Saved Search results to send on for notification
            var result = await handler.Handle(savedSearch);
            return result != null 
                ? new SavedCandidateSearchResultQueueItem { Payload = JsonSerializer.Serialize(result) } 
                : null;
        }
        
        logger.LogInformation("Error occurred while deserializing SavedSearchResult: {Item}", queueItem.Payload);

        return null;
    }
}