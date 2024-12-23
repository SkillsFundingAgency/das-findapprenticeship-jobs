using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Candidate
{
    public class Candidate
    {
        public string GovUkIdentifier { get; set; } = null!;
        public string Email { get; set; } = null!;
        public CandidateStatus Status { get; set; }

        public static implicit operator Candidate(GetCandidatesByActivityApiResponse.Candidate source)
        {
            return new Candidate
            {
                GovUkIdentifier = source.GovUkIdentifier,
                Status = source.Status,
                Email = source.Email,
            };
        }
    }
}