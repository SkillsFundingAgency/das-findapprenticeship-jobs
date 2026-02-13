using System;
using System.Collections.Generic;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.AcceptanceTests.Infrastructure;
public class TestDataValues
{
    private const int PageNo = 1;
    private const int PageSize = 3;
    private static readonly GetLiveVacanciesApiResponse LiveVacancies = new()
    {
        PageNo = PageNo,
        PageSize = PageSize,
        TotalLiveVacanciesReturned = PageSize,
        TotalLiveVacancies = 100,
        TotalPages = 100 / PageSize,
        Vacancies = new List<LiveVacancy>
            {
                new()
                {
                    VacancyId =Guid.NewGuid(),
                    Title = "Vacancy 1",
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
                    Ukprn = 10000528,
                    ProviderName = "BARKING AND DAGENHAM COLLEGE",
                    PostedDate = new DateTime(2023, 10, 05),
                    StandardLarsCode = 91,
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
                new()
                {
                    VacancyId = Guid.NewGuid(),
                    Title = "Vacancy 2",
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
                    Ukprn = 10001309,
                    ProviderName = "COVENTRY AND WARWICKSHIRE CHAMBERS OF COMMERCE TRAINING LIMITED",
                    PostedDate = new DateTime(2023, 10, 15),
                    StandardLarsCode = 561,
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
                new()
                {
                    VacancyId = Guid.NewGuid(),
                    Title = "Vacancy 3",
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
                    Ukprn = 10000528,
                    ProviderName = "COVENTRY AND WARWICKSHIRE CHAMBERS OF COMMERCE TRAINING LIMITED",
                    PostedDate = new DateTime(2023, 10, 15),
                    StandardLarsCode = 160,
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
                },
                new()
                {
                    VacancyId =Guid.NewGuid(),
                    Title = "Vacancy 1",
                    ApprenticeshipTitle = "Apprenticeship title 1",
                    Description = "<ul>\r\n<li>API Testing</li>\r\n<li>Selenium Training</li>\r\n<li>Automation Testing</li>\r\n</ul>",
                    Address = new Address
                    {
                        AddressLine1 = "5 Pine Court",
                        AddressLine2 = "Bristol",
                        AddressLine3 = "Avon",
                        AddressLine4 = null,
                        Postcode = "BS31 2RA",
                        Latitude = 51.40714,
                        Longitude = -2.51436
                    },
                    EmployerName = "Dashkat Consulting Limited",
                    Ukprn = 10000528,
                    ProviderName = "BARKING AND DAGENHAM COLLEGE",
                    PostedDate = new DateTime(2023, 10, 05),
                    StandardLarsCode = 91,
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
                new()
                {
                    VacancyId =Guid.NewGuid(),
                    Title = "Vacancy 1",
                    ApprenticeshipTitle = "Apprenticeship title 1",
                    Description = "<ul>\r\n<li>API Testing</li>\r\n<li>Selenium Training</li>\r\n<li>Automation Testing</li>\r\n</ul>",
                    Address = new Address
                    {
                        AddressLine1 = "Cheylesmore House",
                        AddressLine2 = "5 Quinton Rd",
                        AddressLine3 = "Coventry",
                        AddressLine4 = null,
                        Postcode = "CW1 2WT",
                        Latitude = 52.400347,
                        Longitude = -1.507885
                    },
                    EmployerName = "Dashkat Consulting Limited",
                    Ukprn = 10000528,
                    ProviderName = "BARKING AND DAGENHAM COLLEGE",
                    PostedDate = new DateTime(2023, 10, 05),
                    StandardLarsCode = 91,
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
                new()
                {
                    VacancyId = Guid.NewGuid(),
                    Title = "Vacancy 1000",
                    ApprenticeshipTitle = "Apprenticeship title 1000",
                    Description = "<ul>\r\n<li>API Testing</li>\r\n<li>Selenium Training</li>\r\n<li>Automation Testing</li>\r\n</ul>",
                    EmploymentLocationOption = AvailableWhere.MultipleLocations,
                    EmploymentLocations = 
                    [
                        new Address
                        {
                            AddressLine1 = "5 Pine Court",
                            AddressLine2 = "Bristol",
                            AddressLine3 = "Avon",
                            AddressLine4 = null,
                            Postcode = "BS31 2RA",
                            Latitude = 51.40714,
                            Longitude = -2.51436
                        },
                        new Address
                        {
                            AddressLine1 = "Cheylesmore House",
                            AddressLine2 = "5 Quinton Rd",
                            AddressLine3 = "Coventry",
                            AddressLine4 = null,
                            Postcode = "CW1 2WT",
                            Latitude = 52.400347,
                            Longitude = -1.507885
                        },
                    ],
                    EmployerName = "Dashkat Consulting Limited",
                    Ukprn = 10000528,
                    ProviderName = "BARKING AND DAGENHAM COLLEGE",
                    PostedDate = new DateTime(2023, 10, 05),
                    StandardLarsCode = 91,
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
                new()
                {
                    VacancyId = Guid.NewGuid(),
                    Title = "Vacancy 1001",
                    ApprenticeshipTitle = "Apprenticeship title 1001",
                    Description = "<ul>\r\n<li>API Testing</li>\r\n<li>Selenium Training</li>\r\n<li>Automation Testing</li>\r\n</ul>",
                    EmploymentLocationOption = AvailableWhere.OneLocation,
                    EmploymentLocations = 
                    [
                        new Address
                        {
                            AddressLine1 = "5 Pine Court",
                            AddressLine2 = "Bristol",
                            AddressLine3 = "Avon",
                            AddressLine4 = null,
                            Postcode = "BS31 2RA",
                            Latitude = 51.40714,
                            Longitude = -2.51436
                        }
                    ],
                    EmployerName = "Dashkat Consulting Limited",
                    Ukprn = 10000528,
                    ProviderName = "BARKING AND DAGENHAM COLLEGE",
                    PostedDate = new DateTime(2023, 10, 05),
                    StandardLarsCode = 91,
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
                new()
                {
                    VacancyId = Guid.NewGuid(),
                    Title = "Vacancy 1000",
                    ApprenticeshipTitle = "Apprenticeship title 1000",
                    Description = "<ul>\r\n<li>API Testing</li>\r\n<li>Selenium Training</li>\r\n<li>Automation Testing</li>\r\n</ul>",
                    EmploymentLocationOption = AvailableWhere.AcrossEngland,
                    EmploymentLocationInformation = "Some information about the employment location",
                    EmployerName = "Dashkat Consulting Limited",
                    Ukprn = 10000528,
                    ProviderName = "BARKING AND DAGENHAM COLLEGE",
                    PostedDate = new DateTime(2023, 10, 05),
                    StandardLarsCode = 91,
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
                }
            }
    };

    private static readonly GetNhsLiveVacanciesApiResponse NhsVacancies = new()
    {
        PageNo = PageNo,
        PageSize = PageSize,
        TotalLiveVacanciesReturned = PageSize,
        TotalLiveVacancies = 100,
        TotalPages = 100 / PageSize,
        Vacancies = new List<NhsVacancy>
            {
                new()
                {
                    VacancyId = Guid.NewGuid(),
                    VacancyReference = "VACC9262-24-1846",
                    Title = "NHS Vacancy 1",
                    ApprenticeshipTitle = "AHP IMSK Services - Business & Administration Apprentice",
                    Description = "An exciting opportunity has arisen for highly motivated, enthusiastic and hardworking individuals to join the AHP IMSK Team as a Business and Admin Ap...",
                    ApplicationUrl = "https://beta.jobs.nhs.uk/candidate/jobadvert/C9262-24-1846",
                    Address = new Address
                    {
                        AddressLine1 = null,
                        AddressLine2 = null,
                        AddressLine4 = "Whitehaven",
                        Postcode = "CA28 8JG",
                        Latitude = 54.530059,
                        Longitude = -3.562598
                    },
                    OtherAddresses = [],
                    EmployerName = "North Cumbria Integrated Care NHS Foundation Trust",
                    Ukprn = 0,
                    ProviderName = "",
                    PostedDate = new DateTime(2023, 10, 05),
                    StandardLarsCode = 91,
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
                new()
                {
                    VacancyId = Guid.NewGuid(),
                    VacancyReference = "VACC9186-24-1594",
                    Title = "Apprentice Administration Assistant/Receptionist",
                    ApprenticeshipTitle = "Apprenticeship title 2",
                    Description = "Child and Adolescent Mental Health Services (CAMHS) multi-disciplinary teams provide a comprehensive range of services to children and young people fr...",
                    Address = new Address
                    {
                        AddressLine1 = null,
                        AddressLine2 = null,
                        AddressLine4 = "Langold",
                        Postcode = "S81 9QL",
                        Latitude = 53.376919,
                        Longitude = -1.119084
                    },
                    OtherAddresses = [],
                    EmployerName = "Nottinghamshire Healthcare NHS Foundation Trust",
                    Ukprn = 0,
                    ProviderName = "",
                    PostedDate = new DateTime(2023, 10, 15),
                    StandardLarsCode = 561,
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
            }
    };

    private static readonly GetCivilServiceLiveVacanciesApiResponse CsjVacancies = new()
    {
        PageNo = PageNo,
        PageSize = PageSize,
        TotalLiveVacanciesReturned = PageSize,
        TotalLiveVacancies = 100,
        TotalPages = 100 / PageSize,
        Vacancies = new List<CsjVacancy>
            {
                new()
                {
                    VacancyId = Guid.NewGuid(),
                    VacancyReference = "1593309",
                    Title = "Automation: HMRC - PEC Tests",
                    ApprenticeshipTitle = "AHP IMSK Services - Business & Administration Apprentice",
                    Description = "An exciting opportunity has arisen for highly motivated, enthusiastic and hardworking individuals to join the AHP IMSK Team as a Business and Admin Ap...",
                    ApplicationUrl = "https://cshr-config.tal.net/vx/lang-en-GB/appcentre-11/candidate/postings/4830?instant=apply",
                    Address = new Address
                    {
                        AddressLine1 = null,
                        AddressLine2 = null,
                        AddressLine4 = "Whitehaven",
                        Postcode = "CA28 8JG",
                        Latitude = 54.530059,
                        Longitude = -3.562598
                    },
                    OtherAddresses = [],
                    EmployerName = "Cabinet Office",
                    Ukprn = 0,
                    ProviderName = "",
                    PostedDate = new DateTime(2023, 10, 05),
                    StandardLarsCode = 91,
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
                        WageType = "Fixed",
                        WageText = "75000 to 95000"
                    }
                },
                new()
                {
                    VacancyId = Guid.NewGuid(),
                    VacancyReference = "7894225",
                    Title = "Automation: HMRC - PEC Tests 2",
                    ApprenticeshipTitle = "AHP IMSK Services - Business & Administration Apprentice",
                    Description = "An exciting opportunity has arisen for highly motivated, enthusiastic and hardworking individuals to join the AHP IMSK Team as a Business and Admin Ap...",
                    ApplicationUrl = "https://cshr-config.tal.net/vx/lang-en-GB/appcentre-11/candidate/postings/4830?instant=apply",
                    Address = new Address
                    {
                        AddressLine1 = null,
                        AddressLine2 = null,
                        AddressLine4 = "Whitehaven",
                        Postcode = "CA28 8JG",
                        Latitude = 54.530059,
                        Longitude = -3.562598
                    },
                    OtherAddresses = [],
                    EmployerName = "Cabinet Office",
                    Ukprn = 0,
                    ProviderName = "",
                    PostedDate = new DateTime(2023, 10, 05),
                    StandardLarsCode = 91,
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
                        WageType = "Fixed",
                        WageText = "75000 to 95000"
                    }
                },
            }
    };

    public static readonly ApiResponse<GetLiveVacanciesApiResponse> LiveVacanciesApiResponse = new(body: LiveVacancies,
        System.Net.HttpStatusCode.OK,
        "");

    public static readonly ApiResponse<GetNhsLiveVacanciesApiResponse> NhsVacanciesApiResponse = new(body: NhsVacancies,
        System.Net.HttpStatusCode.OK,
        "");

    public static readonly ApiResponse<GetCivilServiceLiveVacanciesApiResponse> CsjVacanciesApiResponse = new(body: CsjVacancies,
        System.Net.HttpStatusCode.OK,
        "");
}