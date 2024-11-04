using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Models;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class SavedSearchesNotificationsTimerTrigger(
        IGetAllSavedSearchesNotificationHandler handler,
        ILogger<SavedSearchesNotificationsTimerTrigger> log)
    {
        [QueueOutput(StorageQueueNames.SendSavedSearchNotificationAlert)]
        [Function("SendSavedSearchesNotificationsTimerTrigger")]
        public async Task<List<SavedSearchQueueItem>> Run([TimerTrigger("0 0 3 * * MON")] TimerInfo myTimer)
        {
            log.LogInformation($"Send saved searches notifications function executed at: {DateTime.UtcNow}");

            var queueItems = new List<SavedSearchQueueItem>();

            var savedSearches = await handler.Handle();

            queueItems.AddRange(savedSearches.Select(c => new SavedSearchQueueItem
            {
                Categories = c.Categories,
                DisabilityConfident = c.DisabilityConfident,
                Distance = c.Distance,
                Levels = c.Levels,
                SearchTerm = c.SearchTerm,
                User = c.User,
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
}