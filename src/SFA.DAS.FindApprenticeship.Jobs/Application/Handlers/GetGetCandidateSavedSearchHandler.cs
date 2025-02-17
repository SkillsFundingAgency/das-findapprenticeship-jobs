using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;

public class GetGetCandidateSavedSearchHandler(
    IFindApprenticeshipJobsService findApprenticeshipJobsService)
    : IGetGetCandidateSavedSearchHandler
{
    public async Task<SavedSearchCandidateVacancies?> Handle(SavedSearchResult savedSearchResult)
    {
        return await findApprenticeshipJobsService.GetSavedSearchResultsForCandidate(savedSearchResult);
    }
}