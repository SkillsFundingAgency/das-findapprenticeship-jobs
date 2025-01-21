namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers
{
    public interface IUpdateCandidateStatusHandler
    {
        Task BatchHandle(List<Candidate.Candidate> candidates);
    }
}