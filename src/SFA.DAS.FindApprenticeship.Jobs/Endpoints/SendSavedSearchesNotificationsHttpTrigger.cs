using Microsoft.AspNetCore.Http;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Models;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints;

public class SendSavedSearchesNotificationsHttpTrigger(
    IGetAllSavedSearchesNotificationHandler handler,
    ILogger<SendSavedSearchesNotificationsHttpTrigger> log)
{
    [QueueOutput(StorageQueueNames.SendSavedSearchNotificationAlert)]
    [Function("SendSavedSearchesNotificationsHttp")]
    public async Task<List<SavedSearchQueueItem>> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        log.LogInformation("Send Saved Searches Notifications HTTP function executed at: {DateTime}", DateTime.UtcNow);
        var queueItems = new List<SavedSearchQueueItem>();

        var savedSearches = await handler.Handle();

        queueItems.AddRange(savedSearches.Select(c => new SavedSearchQueueItem
        {
            SearchTerm = c.SearchTerm,
            Location = c.Location,
            DisabilityConfident = c.DisabilityConfident,
            Distance = c.Distance,
            UnSubscribeToken = c.UnSubscribeToken,
            User = c.User,
            Levels = c.Levels?.Select(lev => (SavedSearchQueueItem.Level)lev).ToList(),
            Categories = c.Categories?.Select(cat => (SavedSearchQueueItem.Category)cat).ToList(),
            Vacancies = c.Vacancies.Select(vac => new SavedSearchQueueItem.Vacancy
            {
                Address = vac.Address,
                ClosingDate = vac.ClosingDate,
                Distance = vac.Distance,
                EmployerName = vac.EmployerName,
                Title = vac.Title,
                TrainingCourse = vac.TrainingCourse,
                Id = vac.Id,
                VacancyReference = vac.VacancyReference,
                Wage = vac.Wage
            }).ToList()
        }));

        return queueItems;
    }
}