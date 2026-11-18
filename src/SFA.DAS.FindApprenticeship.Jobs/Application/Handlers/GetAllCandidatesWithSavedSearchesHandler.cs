using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

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
        var recordsToDelete = new List<GetCandidateSavedSearchesApiResponse.SavedSearchResult>();
            
        while (pageNumber <= savedSearchesResponse.TotalPages)
        {
            if (savedSearchesResponse is not { SavedSearchResults.Count: > 0 })
            {
                break;
            }
                
            pageNumber++;

            recordsToDelete.AddRange(savedSearchesResponse.SavedSearchResults.Where(x => x.HasInvalidSearchParameters));
            savedSearchesNotifications.AddRange(savedSearchesResponse.SavedSearchResults.Where(x => !x.HasInvalidSearchParameters).Select(x => (SavedSearchResult)x));
            savedSearchesResponse = await findApprenticeshipJobsService.GetSavedSearches(pageNumber, PageSize, lastRunDateTime.ToString("O"));
        }

        if (recordsToDelete is { Count: >0 })
        {
            await findApprenticeshipJobsService.DeleteSavedSearchesAsync(recordsToDelete.Select(x => x.Id));
        }
        savedSearchesNotifications.ForEach(i => i.LastRunDate = lastRunDateTime);
        return savedSearchesNotifications;
    }
}