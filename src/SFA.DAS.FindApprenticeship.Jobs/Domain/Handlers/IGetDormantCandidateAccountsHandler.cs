namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;

public interface IGetDormantCandidateAccountsHandler
{
    Task<List<Candidate.Candidate>> Handle();
}