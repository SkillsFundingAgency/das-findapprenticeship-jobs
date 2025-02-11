using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests
{
    public class GetInactiveCandidatesApiRequest(string cutOffDateTime, int pageNumber, int pageSize) : IGetApiRequest
    {
        public string GetUrl => $"Candidates/GetInactiveCandidates?cutOffDateTime={cutOffDateTime}&pageSize={pageSize}&pageNo={pageNumber}";
    }
}