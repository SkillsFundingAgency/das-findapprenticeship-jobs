using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Models
{
    public class SavedSearchQueueItem
    {
        public UserDetails? User { get; set; } = new();
        public List<string>? Categories { get; set; }

        public List<string>? Levels { get; set; }

        public int Distance { get; set; }

        public string? SearchTerm { get; set; }

        public bool DisabilityConfident { get; set; }

        public List<Vacancy>? Vacancies { get; set; } = [];
        public string? Location { get; set; }

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

            public static implicit operator Vacancy(SavedSearch.Vacancy source)
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

            public static implicit operator Address(SavedSearch.Address source)
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

            public static implicit operator UserDetails(SavedSearch.UserDetails source)
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
    }
}