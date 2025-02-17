using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;

namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;

public interface IGetGetCandidateSavedSearchHandler
{
    Task<SavedSearchCandidateVacancies?> Handle(SavedSearchResult savedSearchResult);
}