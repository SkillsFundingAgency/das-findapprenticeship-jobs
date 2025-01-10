using Newtonsoft.Json;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses
{
    public class GetInactiveCandidatesApiResponse
    {
        [JsonProperty("Candidates")]
        public List<Candidate> Candidates { get; set; } = [];
        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }

        [JsonProperty("pageIndex")]
        public int PageIndex { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }

        public class Candidate
        {
            [JsonProperty("govUkIdentifier")] 
            public string GovUkIdentifier { get; set; } = null!;

            [JsonProperty("email")] 
            public string Email { get; set; } = null!;
        }
    }
}
