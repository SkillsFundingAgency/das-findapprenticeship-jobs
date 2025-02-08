using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;

public class PostGetSavedSearchResultsForCandidateRequest(SavedSearchResult savedSearchCandidateVacancies)
    : IPostApiRequestWithData
{
    public string PostUrl => "savedSearches/GetSavedSearchResult";
    public object Data { get; set; } = savedSearchCandidateVacancies;
}