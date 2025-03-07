using System.Text.Json.Serialization;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

public class GetCandidateSavedSearchesApiResponse
{
    [JsonPropertyName("savedSearchResults")]
    public List<SavedSearchResult> SavedSearchResults { get; set; } = new();

    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    [JsonPropertyName("pageIndex")]
    public int PageIndex { get; set; }
    
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }
        
    [JsonPropertyName("lastRunDateFilter")]
    public DateTime LastRunDateFilter { get; set; }

    public class SavedSearchResult
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
            
        [JsonPropertyName("userId")] 
        public Guid UserId { get; set; }

        [JsonPropertyName("distance")]
        public decimal? Distance { get; set; }

        [JsonPropertyName("searchTerm")]
        public string? SearchTerm { get; set; }

        [JsonPropertyName("location")]
        public string? Location { get; set; }

        [JsonPropertyName("disabilityConfident")]
        public bool DisabilityConfident { get; set; }
            
        [JsonPropertyName("longitude")]
        public string? Longitude { get; set; }
            
        [JsonPropertyName("latitude")]
        public string? Latitude { get; set; }
            
        [JsonPropertyName("selectedRouteIds")] 
        public List<int>? SelectedRouteIds { get; set; }

        [JsonPropertyName("selectedLevelIds")]
        public List<int>? SelectedLevelIds { get; set; } 

        [JsonPropertyName("unSubscribeToken")]
        public string? UnSubscribeToken { get; set; }

    }

}