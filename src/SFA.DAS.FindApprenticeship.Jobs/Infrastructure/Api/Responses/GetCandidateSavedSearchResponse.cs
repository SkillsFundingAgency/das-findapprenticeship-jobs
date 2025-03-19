using System.Text.Json.Serialization;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

public class GetCandidateSavedSearchResponse
{

    [JsonPropertyName("id")] 
    public Guid Id { get; set; }

    [JsonPropertyName("user")] 
    public UserDetails User { get; set; } = new();

    [JsonPropertyName("categories")] 
    public List<Category>? Categories { get; set; }

    [JsonPropertyName("levels")] 
    public List<Level>? Levels { get; set; }

    [JsonPropertyName("distance")] 
    public int? Distance { get; set; }

    [JsonPropertyName("searchTerm")] 
    public string? SearchTerm { get; set; }

    [JsonPropertyName("location")] 
    public string? Location { get; set; }

    [JsonPropertyName("disabilityConfident")]
    public bool DisabilityConfident { get; set; }

    [JsonPropertyName("excludeNational")]
    public bool? ExcludeNational { get; set; }

    [JsonPropertyName("unSubscribeToken")] 
    public string? UnSubscribeToken { get; set; }

    [JsonPropertyName("vacancies")] 
    public List<Vacancy> Vacancies { get; set; } = [];
    public class UserDetails
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("firstName")]
        public string? FirstName { get; set; }
        [JsonPropertyName("middleNames")]
        public string? MiddleNames { get; set; }
        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }
        [JsonPropertyName("email")]
        public string? Email { get; set; }
    }

    public class Category
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
    public class Level
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

    public class Vacancy
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("vacancyReference")]
        public string? VacancyReference { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("employerName")]
        public string? EmployerName { get; set; }

        [JsonPropertyName("address")]
        public VacancyAddress? Address { get; set; }

        [JsonPropertyName("otherAddresses")] 
        public List<VacancyAddress> OtherAddresses { get; set; } = [];
        [JsonPropertyName("employmentLocationInformation")]
        public string? EmploymentLocationInformation { get; set; }

        [JsonPropertyName("employmentLocationOption")]
        public string? EmploymentLocationOption { get; set; }

        [JsonPropertyName("wage")]
        public string? Wage { get; set; }

        [JsonPropertyName("closingDate")]
        public string? ClosingDate { get; set; }
        
        [JsonPropertyName("startDate")]
        public string? StartDate { get; set; }

        [JsonPropertyName("trainingCourse")]
        public string? TrainingCourse { get; set; }

        [JsonPropertyName("distance")]
        public double? Distance { get; set; }
            
        [JsonPropertyName("vacancySource")]
        public string? VacancySource { get; set; }
        [JsonPropertyName("wageUnit")]
        public string? WageUnit { get; set; }
        [JsonPropertyName("wageType")]
        public string? WageType { get; set; }
            
            
    }
    public class VacancyAddress
    {
        [JsonPropertyName("addressLine1")]
        public string? AddressLine1 { get; set; }
            
        [JsonPropertyName("addressLine2")]
        public string? AddressLine2 { get; set; }
            
        [JsonPropertyName("addressLine3")]
        public string? AddressLine3 { get; set; }
            
        [JsonPropertyName("addressLine4")]
        public string? AddressLine4 { get; set; }
            
        [JsonPropertyName("postcode")]
        public string? Postcode { get; set; }
    }
}