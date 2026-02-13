using System.Text.Json;
using Microsoft.AspNetCore.Http;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Models;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints;

public class SendSavedSearchesNotificationsHttpTrigger(
    IGetAllCandidatesWithSavedSearchesHandler handler,
    ILogger<SendSavedSearchesNotificationsHttpTrigger> log)
{
    [QueueOutput(StorageQueueNames.GetSavedSearchNotifications)]
    [Function("SendSavedSearchesNotificationsHttp")]
    public async Task<List<SavedSearchQueueItem>> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        log.LogInformation("Send Saved Searches Notifications HTTP function executed at: {DateTime}", DateTime.UtcNow);

        var savedSearches = await handler.Handle();

        return savedSearches.Select(savedSearch => new SavedSearchQueueItem { Payload = JsonSerializer.Serialize(savedSearch) }).ToList();
    }
}