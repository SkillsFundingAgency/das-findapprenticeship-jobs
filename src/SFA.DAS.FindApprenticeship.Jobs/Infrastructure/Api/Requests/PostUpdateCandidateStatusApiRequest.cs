using SFA.DAS.FindApprenticeship.Jobs.Domain.Candidate;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests
{
    public class PostUpdateCandidateStatusApiRequest(string govIdentifier, CandidateUpdateStatusRequest data) : IPostApiRequestWithData
    {
        public string PostUrl => $"Candidates/{govIdentifier}/status";
        public object Data { get; set; } = data;
    }
}