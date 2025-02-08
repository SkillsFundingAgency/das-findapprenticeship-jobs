using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers
{
    public class GetAllSavedSearchesNotificationHandler(
        IDateTimeService dateTimeService,
        IFindApprenticeshipJobsService findApprenticeshipJobsService)
        : IGetAllCandidatesWithSavedSearchesHandler
    {
        private const int PageSize = 1000;


        public async Task<List<SavedSearchResult>> Handle()
        {
            var pageNumber = 1;
            var totalPages = 100;

            var lastRunDateTime = dateTimeService.GetCurrentDateTime().AddDays(-7);
            var batchSavedSearchesNotifications = new List<SavedSearchResult>();

            while (pageNumber <= totalPages)
            {
                var savedSearchesResponse = await findApprenticeshipJobsService.GetSavedSearches(pageNumber,
                    PageSize,
                    lastRunDateTime.ToString("O"));

                if (savedSearchesResponse is { SavedSearchResults.Count: > 0 })
                {
                    totalPages = savedSearchesResponse.TotalPages;

                    batchSavedSearchesNotifications =
                        savedSearchesResponse.SavedSearchResults.Select(x => (SavedSearchResult)x).ToList();

                    pageNumber++;
                }
                else
                {
                    break;
                }
            }
            
            batchSavedSearchesNotifications.ToList().ForEach(i => i.LastRunDate = lastRunDateTime);
            
            return batchSavedSearchesNotifications;
        }
    }
}
