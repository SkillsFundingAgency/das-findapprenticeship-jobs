using SFA.DAS.FindApprenticeship.Jobs.Domain.Candidate;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Models
{
    public class UpdateCandidateStatusQueueItem
    {
        public List<Candidate> Candidates { get; set; } = [];
    }
}