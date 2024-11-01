using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints
{
    public class SavedSearchesNotificationsTimerTrigger
    {
        private readonly IGetAllSavedSearchesNotificationHandler _handler;

        public SavedSearchesNotificationsTimerTrigger(IGetAllSavedSearchesNotificationHandler handler)
        {
            _handler = handler;
        }

        [FunctionName("SendSavedSearchesNotificationsTimerTrigger")]
        public async Task Run([TimerTrigger("0 0 3 ? * MON *")] TimerInfo myTimer,
            ILogger log,
            [Queue(StorageQueueNames.SendSavedSearchNotificationAlert)]
            ICollector<SavedSearchQueueItem> outputQueue)
        {
            log.LogInformation($"Send saved searches notifications function executed at: {DateTime.UtcNow}");

            var queueItems = new List<SavedSearchQueueItem>();

            var savedSearches = await _handler.Handle();

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

            foreach (var savedSearchQueueItem in queueItems)
            {
                outputQueue.Add(savedSearchQueueItem);
            }
        }
    }
}