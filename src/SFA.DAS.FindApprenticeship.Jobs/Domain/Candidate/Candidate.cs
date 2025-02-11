using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Candidate
{
    public class Candidate
    {
        public string GovUkIdentifier { get; set; } = null!;
        public string Email { get; set; } = null!;

        public static implicit operator Candidate(GetInactiveCandidatesApiResponse.Candidate source)
        {
            return new Candidate
            {
                GovUkIdentifier = source.GovUkIdentifier,
                Email = source.Email,
            };
        }
    }
}