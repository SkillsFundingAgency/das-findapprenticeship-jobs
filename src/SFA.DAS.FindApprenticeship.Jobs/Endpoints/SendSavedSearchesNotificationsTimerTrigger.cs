using System.Text.Json;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Models;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints;

public class SavedSearchesNotificationsTimerTrigger(
    IGetAllCandidatesWithSavedSearchesHandler handler,
    ILogger<SavedSearchesNotificationsTimerTrigger> logger)
{
    [QueueOutput(StorageQueueNames.GetSavedSearchNotifications)]
    [Function("SendSavedSearchesNotificationsTimerTrigger")]
    public async Task<List<SavedSearchQueueItem>> Run([TimerTrigger("0 0 3 * * MON")] TimerInfo myTimer)
    {
        logger.LogInformation("Get saved searches notifications function executed at: {DateTime}", DateTime.UtcNow);

        var savedSearches = await handler.Handle();

        return savedSearches.Select(savedSearch => new SavedSearchQueueItem { Payload = JsonSerializer.Serialize(savedSearch) }).ToList();
    }
}