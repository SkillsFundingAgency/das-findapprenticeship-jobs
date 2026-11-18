using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests
{
    public class GetSavedSearchesApiRequest(int? pageNumber, int? pageSize, string? lastRunDateTime) : IGetApiRequest
    {
        public string GetUrl { get; } = $"savedSearches?pageSize={pageSize}&pageNumber={pageNumber}&lastRunDateTime={lastRunDateTime}";
    }
}
