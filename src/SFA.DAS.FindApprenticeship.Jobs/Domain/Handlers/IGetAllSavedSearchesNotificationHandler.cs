using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;

namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;

public interface IGetAllCandidatesWithSavedSearchesHandler
{
    Task<List<SavedSearchResult>> Handle();
}