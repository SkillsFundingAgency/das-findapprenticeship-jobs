namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Candidate
{
    public class CandidateUpdateStatusRequest
    {
        public required string Email { get; set; }
        public CandidateStatus Status { get; set; }
    }
}