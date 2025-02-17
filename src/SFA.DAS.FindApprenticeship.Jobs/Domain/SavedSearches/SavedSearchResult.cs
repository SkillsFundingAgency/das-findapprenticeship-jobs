using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;

public class SavedSearchResult
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal? Distance { get; set; }
    public string? SearchTerm { get; set; }
    public string? Location { get; set; }
    public bool DisabilityConfident { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
    public List<int>? SelectedLevelIds { get; set; } = [];
    public List<int>? SelectedRouteIds { get; set; } = [];
    public string? UnSubscribeToken { get; set; }
    public DateTime LastRunDate { get; set; }

    public static implicit operator SavedSearchResult(
        GetCandidateSavedSearchesApiResponse.SavedSearchResult searchResult)
    {
        return new SavedSearchResult
        {
            Id = searchResult.Id,
            UserId = searchResult.UserId,
            Distance = searchResult.Distance,
            SearchTerm = searchResult.SearchTerm,
            Location = searchResult.Location,
            DisabilityConfident = searchResult.DisabilityConfident,
            Longitude = searchResult.Longitude,
            Latitude = searchResult.Latitude,
            SelectedLevelIds = searchResult.SelectedLevelIds,
            SelectedRouteIds = searchResult.SelectedRouteIds,
            UnSubscribeToken = searchResult.UnSubscribeToken
        };
    }
}