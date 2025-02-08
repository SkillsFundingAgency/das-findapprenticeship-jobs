using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches
{
    public class SavedSearchCandidateVacancies
    {
        public Guid Id { get; set; }
        public UserDetails User { get; set; } = new();
        public List<Category>? Categories { get; set; }
        public List<Level>? Levels { get; set; }
        public decimal? Distance { get; set; }
        public string? SearchTerm { get; set; }
        public string? Location { get; set; }
        public bool DisabilityConfident { get; set; }
        public string? UnSubscribeToken { get; set; }
        public List<Vacancy> Vacancies { get; set; } = [];

        public static implicit operator SavedSearchCandidateVacancies(GetCandidateSavedSearchResult.SavedSearchResult source)
        {
            return new SavedSearchCandidateVacancies
            {
                Id = source.Id,
                User = source.User,
                Categories = source.Categories?.Select(cat => (Category)cat).ToList(),
                Levels = source.Levels?.Select(lev => (Level)lev).ToList(),
                Distance = source.Distance,
                SearchTerm = source.SearchTerm,
                Location = source.Location,
                DisabilityConfident = source.DisabilityConfident,
                UnSubscribeToken = source.UnSubscribeToken,
                Vacancies = source.Vacancies.Select(x => (Vacancy) x).ToList()
            };
        }

        public class Vacancy
        {
            public string? Id { get; set; }

            public string? VacancyReference { get; set; }

            public string? Title { get; set; }

            public string? EmployerName { get; set; }

            public Address Address { get; set; } = new();

            public string? Wage { get; set; }

            public string? ClosingDate { get; set; }

            public string? TrainingCourse { get; set; }

            public double? Distance { get; set; }
            
            public string? VacancySource { get; set; }
            public string? WageUnit { get; set; }
            public string? WageType { get; set; }

            public static implicit operator Vacancy(GetCandidateSavedSearchResult.Vacancy source)
            {
                return new Vacancy
                {
                    Id = source.Id,
                    VacancyReference = source.VacancyReference,
                    Title = source.Title,
                    EmployerName = source.EmployerName,
                    Wage = source.Wage,
                    ClosingDate = source.ClosingDate,
                    TrainingCourse = source.TrainingCourse,
                    Distance = source.Distance,
                    Address = source.Address,
                    VacancySource = source.VacancySource,
                    WageUnit = source.WageUnit,
                    WageType = source.WageType
                };
            }
        }

        public class Address
        {
            public string? AddressLine1 { get; set; }

            public string? AddressLine2 { get; set; }

            public string? AddressLine3 { get; set; }

            public string? AddressLine4 { get; set; }

            public string? Postcode { get; set; }

            public static implicit operator Address(GetCandidateSavedSearchResult.Address source)
            {
                return new Address
                {
                    AddressLine1 = source.AddressLine1,
                    AddressLine2 = source.AddressLine2,
                    AddressLine3 = source.AddressLine3,
                    AddressLine4 = source.AddressLine4,
                    Postcode = source.Postcode,
                };
            }
        }

        public class UserDetails
        {
            public Guid Id { get; set; }
            public string? FirstName { get; set; }
            public string? MiddleNames { get; set; }
            public string? LastName { get; set; }
            public string? Email { get; set; }

            public static implicit operator UserDetails(GetCandidateSavedSearchResult.UserDetails source)
            {
                return new UserDetails
                {
                    Id = source.Id,
                    FirstName = source.FirstName,
                    MiddleNames = source.MiddleNames,
                    LastName = source.LastName,
                    Email = source.Email,
                };
            }
        }

        public class Category
        {
            public int Id { get; set; }
            public string? Name { get; set; }

            public static implicit operator Category(GetCandidateSavedSearchResult.Category source)
            {
                return new Category
                {
                    Id = source.Id,
                    Name = source.Name,
                };
            }
        }

        public class Level
        {
            public int Code { get; set; }
            public string? Name { get; set; }

            public static implicit operator Level(GetCandidateSavedSearchResult.Level source)
            {
                return new Level
                {
                    Code = source.Code,
                    Name = source.Name,
                };
            }
        }
    }
}