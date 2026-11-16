using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;

public record PostDeleteSavedSearchesRequest(IEnumerable<Guid> Ids) : IPostApiRequestWithData
{
    public string PostUrl => "savedSearches/delete-many";
    public object Data { get; set; } = Ids;
}