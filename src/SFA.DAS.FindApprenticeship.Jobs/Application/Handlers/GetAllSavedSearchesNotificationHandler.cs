using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers
{
    public class GetAllSavedSearchesNotificationHandler(
        IDateTimeService dateTimeService,
        IFindApprenticeshipJobsService findApprenticeshipJobsService)
        : IGetAllSavedSearchesNotificationHandler
    {
        private const int MaxApprenticeshipSearchResultCount = 5;
        private const string SortOrder = "AgeDesc";
        private const int PageSize = 1000;


        public async Task<List<SavedSearch>> Handle()
        {
            var pageNumber = 1;
            var totalPages = 100;

            var lastRunDateTime = dateTimeService.GetCurrentDateTime().AddDays(-7);
            var batchSavedSearchesNotifications = new List<SavedSearch>();

            while (pageNumber <= totalPages)
            {
                var savedSearchesResponse = await findApprenticeshipJobsService.GetSavedSearches(pageNumber,
                    PageSize,
                    lastRunDateTime.ToString("O"),
                    MaxApprenticeshipSearchResultCount,
                    SortOrder);

                if (savedSearchesResponse is { SavedSearchResults.Count: > 0 })
                {
                    totalPages = savedSearchesResponse.TotalPages;

                    batchSavedSearchesNotifications =
                        savedSearchesResponse.SavedSearchResults.Select(x => (SavedSearch)x).ToList();

                    pageNumber++;
                }
                else
                {
                    break;
                }
            }

            return batchSavedSearchesNotifications;
        }
    }
}
