using Newtonsoft.Json;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses
{
    public class GetSavedSearchesApiResponse
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

        public class SavedSearchResult
        {
            [JsonProperty("id")]
            public Guid Id { get; set; }
            
            [JsonProperty("userDetails")] 
            public UserDetails User { get; set; } = new();

            [JsonProperty("categories")] 
            public List<Category>? Categories { get; set; }

            [JsonProperty("levels")]
            public List<Level>? Levels { get; set; } 

            [JsonProperty("distance")]
            public int? Distance { get; set; }

            [JsonProperty("searchTerm")]
            public string? SearchTerm { get; set; }

            [JsonProperty("location")]
            public string? Location { get; set; }

            [JsonProperty("disabilityConfident")]
            public bool DisabilityConfident { get; set; }

            [JsonProperty("unSubscribeToken")]
            public string? UnSubscribeToken { get; set; }

            [JsonProperty("vacancies")] 
            public List<Vacancy> Vacancies { get; set; } = [];
        }

        public class UserDetails
        {
            [JsonProperty("id")]
            public Guid Id { get; set; }
            [JsonProperty("firstName")]
            public string? FirstName { get; set; }
            [JsonProperty("middleNames")]
            public string? MiddleNames { get; set; }
            [JsonProperty("lastName")]
            public string? LastName { get; set; }
            [JsonProperty("email")]
            public string? Email { get; set; }
        }

        public class Category
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("name")]
            public string? Name { get; set; }
        }
        public class Level
        {
            [JsonProperty("code")]
            public int Code { get; set; }
            [JsonProperty("name")]
            public string? Name { get; set; }
        }

        public class Vacancy
        {
            [JsonProperty("id")]
            public string? Id { get; set; }

            [JsonProperty("vacancyReference")]
            public string? VacancyReference { get; set; }

            [JsonProperty("title")]
            public string? Title { get; set; }

            [JsonProperty("employerName")]
            public string? EmployerName { get; set; }

            [JsonProperty("address")]
            public Address Address { get; set; } = new();

            [JsonProperty("wage")]
            public string? Wage { get; set; }

            [JsonProperty("closingDate")]
            public string? ClosingDate { get; set; }

            [JsonProperty("trainingCourse")]
            public string? TrainingCourse { get; set; }

            [JsonProperty("distance")]
            public double? Distance { get; set; }
            
            [JsonProperty("vacancySource")]
            public string? VacancySource { get; set; }
            [JsonProperty("wageUnit")]
            public string? WageUnit { get; set; }
            [JsonProperty("wageType")]
            public string? WageType { get; set; }
        }

        public class Address
        {
            [JsonProperty("addressLine1")]
            public string? AddressLine1 { get; set; }

            [JsonProperty("addressLine2")]
            public string? AddressLine2 { get; set; }

            [JsonProperty("addressLine3")]
            public string? AddressLine3 { get; set; }

            [JsonProperty("addressLine4")]
            public string? AddressLine4 { get; set; }

            [JsonProperty("postcode")]
            public string? Postcode { get; set; }
        }
    }
}
