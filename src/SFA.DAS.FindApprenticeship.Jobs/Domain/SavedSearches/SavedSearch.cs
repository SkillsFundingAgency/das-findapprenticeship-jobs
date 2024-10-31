using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches
{
    public class SavedSearch
    {
        public UserDetails User { get; set; } = new();
        public List<int>? Categories { get; set; }

        public List<int>? Levels { get; set; }

        public int Distance { get; set; }

        public string? SearchTerm { get; set; }

        public bool DisabilityConfident { get; set; }

        public List<Vacancy> Vacancies { get; set; } = new();

        public static implicit operator SavedSearch(GetSavedSearchesApiResponse.SavedSearchResult source)
        {
            return new SavedSearch
            {
                User = source.UserDetails,
                Categories = source.Categories,
                Levels = source.Levels,
                Distance = source.Distance,
                SearchTerm = source.SearchTerm,
                DisabilityConfident = source.DisabilityConfident,
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

            public static implicit operator Vacancy(GetSavedSearchesApiResponse.Vacancy source)
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

            public static implicit operator Address(GetSavedSearchesApiResponse.Address source)
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

            public static implicit operator UserDetails(GetSavedSearchesApiResponse.UserDetails source)
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