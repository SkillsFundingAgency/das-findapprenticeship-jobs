using System;
using System.Collections.Generic;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.AcceptanceTests.Infrastructure;
public class TestDataValues
{
    public const int PageNo = 1;
    public const int PageSize = 3;
    public static ApiResponse<GetLiveVacanciesApiResponse> LiveVacanciesApiResponse = new ApiResponse<GetLiveVacanciesApiResponse>(LiveVacancies, System.Net.HttpStatusCode.OK, "");
    public static GetLiveVacanciesApiResponse LiveVacancies = new GetLiveVacanciesApiResponse()
    {
        PageNo = PageNo,
        PageSize = PageSize,
        TotalLiveVacanciesReturned = PageSize,
        TotalLiveVacancies = 100,
        TotalPages = 100 / PageSize,
        Vacancies = new List<LiveVacancy>()
            {
                new LiveVacancy()
                {
                    VacancyId =Guid.NewGuid(),
                    VacancyTitle = "Vacancy 1",
                    ApprenticeshipTitle = "Apprenticeship title 1",
                    Description = "<ul>\r\n<li>API Testing</li>\r\n<li>Selenium Training</li>\r\n<li>Automation Testing</li>\r\n</ul>",
                    Address = new Address
                    {
                        AddressLine1 = "White Hart House High Street",
                        AddressLine2 = "Limpsfield",
                        Postcode = "RH8 0DT",
                        Latitude = 51.258114,
                        Longitude = 0.014037
                    },
                    EmployerName = "Dashkat Consulting Limited",
                    ProviderId = 10000528,
                    ProviderName = "BARKING AND DAGENHAM COLLEGE",
                    LiveDate = new DateTime(2023, 10, 05),
                    ProgrammeId = "91",
                    ProgrammeType = "Standard",
                    StartDate = new DateTime(2025, 02, 01),
                    ClosingDate = new DateTime(2025, 01, 01),
                    Route = "",
                    Level = 4,
                    Wage = new Wage
                    {
                        Duration = 2,
                        DurationUnit = "Year",
                        WorkingWeekDescription = "Monday to Friday 9am to 5pm, shifts, may work evenings and weekends.",
                        WeeklyHours = 35,
                        WageType = "Competitive Salary"
                    }
                },
                new LiveVacancy()
                {
                    VacancyId = Guid.NewGuid(),
                    VacancyTitle = "Vacancy 2",
                    ApprenticeshipTitle = "Apprenticeship title 2",
                    Description = "<ul>\r\n<li>Advance Mathematics</li>\r\n<li>Computer Modelling</li>\r\n<li>Speech Recognition</li>\r\n</ul>\r\n<p>5day a week training</p>",
                    Address = new Address
                    {
                        AddressLine1 = "White Hart House High Street",
                        AddressLine2 = "Limpsfield",
                        Postcode = "RH8 0DT",
                        Latitude = 51.258114,
                        Longitude = 0.014037
                    },
                    EmployerName = "Dashkat Consulting Limited",
                    ProviderId = 10001309,
                    ProviderName = "COVENTRY AND WARWICKSHIRE CHAMBERS OF COMMERCE TRAINING LIMITED",
                    LiveDate = new DateTime(2023, 10, 15),
                    ProgrammeId = "561",
                    ProgrammeType = "Standard",
                    StartDate = new DateTime(2025, 05, 11),
                    ClosingDate = new DateTime(2025, 02, 01),
                    Route = "",
                    Level = 7,
                    Wage = new Wage
                    {
                        Duration = 12,
                        DurationUnit = "Month",
                        WorkingWeekDescription = "Monday to Friday 9am to 5pm, shifts, may work evenings and weekends.",
                        WeeklyHours = 35,
                        WageType = "Competitive Salary",
                        WageAdditionalInformation = "pay information about bonus"
                    }
                },
                new LiveVacancy()
                {
                    VacancyId = Guid.NewGuid(),
                    VacancyTitle = "Vacancy 3",
                    ApprenticeshipTitle = "Apprenticeship title 3",
                    Description = "<p>Write a few sentences about the apprenticeship to attract applicants. This information will appear under your advert title when applicants look through a list of adverts - make it stand out.</p>",
                    Address = new Address
                    {
                        AddressLine1 = "White Hart House High Street",
                        AddressLine2 = "Limpsfield",
                        Postcode = "RH8 0DT",
                        Latitude = 51.258114,
                        Longitude = 0.014037
                    },
                    EmployerName = "Dashkat Consulting Limited",
                    ProviderId = 10000528,
                    ProviderName = "COVENTRY AND WARWICKSHIRE CHAMBERS OF COMMERCE TRAINING LIMITED",
                    LiveDate = new DateTime(2023, 10, 15),
                    ProgrammeId = "160",
                    ProgrammeType = "Standard",
                    StartDate = new DateTime(2025, 01, 01),
                    ClosingDate = new DateTime(2024, 11, 01),
                    Route = "",
                    Level = 5,
                    Wage = new Wage
                    {
                        Duration = 1,
                        DurationUnit = "Year",
                        WorkingWeekDescription = "Monday to Friday 9am to 5pm, shifts, may work evenings and weekends.",
                        WeeklyHours = 35,
                        WageType = "Competitive Salary",
                        WageAdditionalInformation = "all about bonuses"
                    }
                }
            }
    };
}