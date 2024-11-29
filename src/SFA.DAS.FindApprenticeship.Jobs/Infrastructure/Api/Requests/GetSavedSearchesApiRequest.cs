using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests
{
    public class GetSavedSearchesApiRequest : IGetApiRequest
    {
        public GetSavedSearchesApiRequest(int? pageNumber, int? pageSize, string? lastRunDateTime, int? maxApprenticeshipSearchResultCount)
        {
            GetUrl = $"savedSearches?pageSize={pageSize}&pageNo={pageNumber}&lastRunDateTime={lastRunDateTime}&maxApprenticeshipSearchResultCount={maxApprenticeshipSearchResultCount}"; 
        }

        public string GetUrl { get; }
    }
}
