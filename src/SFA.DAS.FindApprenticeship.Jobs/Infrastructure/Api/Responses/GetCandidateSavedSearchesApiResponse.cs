using Newtonsoft.Json;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses
{
    public class GetCandidateSavedSearchesApiResponse
    {
        [JsonProperty("savedSearchResults")]
        public List<SavedSearchResult> SavedSearchResults { get; set; } = new();

        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }

        [JsonProperty("pageIndex")]
        public int PageIndex { get; set; }
    
        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }
        
        [JsonProperty("lastRunDateFilter")]
        public DateTime LastRunDateFilter { get; set; }

        public class SavedSearchResult
        {
            [JsonProperty("id")]
            public Guid Id { get; set; }
            
            [JsonProperty("userId")] 
            public Guid UserId { get; set; }

            [JsonProperty("distance")]
            public decimal? Distance { get; set; }

            [JsonProperty("searchTerm")]
            public string? SearchTerm { get; set; }

            [JsonProperty("location")]
            public string? Location { get; set; }

            [JsonProperty("disabilityConfident")]
            public bool DisabilityConfident { get; set; }
            
            [JsonProperty("longitude")]
            public string? Longitude { get; set; }
            
            [JsonProperty("longitude")]
            public string? Latitude { get; set; }
            
            [JsonProperty("selectedRouteIds")] 
            public List<int>? SelectedRouteIds { get; set; }

            [JsonProperty("selectedLevelIds")]
            public List<int>? SelectedLevelIds { get; set; } 

            [JsonProperty("unSubscribeToken")]
            public string? UnSubscribeToken { get; set; }

        }

    }
}
