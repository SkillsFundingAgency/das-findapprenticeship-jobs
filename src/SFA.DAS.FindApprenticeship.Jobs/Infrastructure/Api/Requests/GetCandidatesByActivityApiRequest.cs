using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests
{
    public class GetCandidatesByActivityApiRequest(string cutOffDateTime, int pageNumber, int pageSize) : IGetApiRequest
    {
        public string GetUrl => $"Candidates/GetCandidatesByActivity?cutOffDateTime={cutOffDateTime}&pageSize={pageSize}&pageNo={pageNumber}";
    }
}