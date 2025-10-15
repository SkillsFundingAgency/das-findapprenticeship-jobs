using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;

public class GetAllCandidatesWithSavedSearchesHandler(
    IDateTimeService dateTimeService,
    IFindApprenticeshipJobsService findApprenticeshipJobsService)
    : IGetAllCandidatesWithSavedSearchesHandler
{
    private const int PageSize = 1000;


    public async Task<List<SavedSearchResult>> Handle()
    {
        var pageNumber = 1;
        var lastRunDateTime = dateTimeService.GetCurrentDateTime().AddDays(-7);
        var savedSearchesNotifications = new List<SavedSearchResult>();
        var savedSearchesResponse = await findApprenticeshipJobsService.GetSavedSearches(pageNumber, PageSize, lastRunDateTime.ToString("O"));
            
        while (pageNumber <= savedSearchesResponse.TotalPages)
        {
            if (savedSearchesResponse is not { SavedSearchResults.Count: > 0 })
            {
                break;
            }
                
            pageNumber++;
            savedSearchesNotifications.AddRange(savedSearchesResponse.SavedSearchResults.Select(x => (SavedSearchResult)x));
            savedSearchesResponse = await findApprenticeshipJobsService.GetSavedSearches(pageNumber, PageSize, lastRunDateTime.ToString("O"));
        }
            
        savedSearchesNotifications.ForEach(i => i.LastRunDate = lastRunDateTime);
        return savedSearchesNotifications;
    }
}