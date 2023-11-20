using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Domain;
public class WhenCastingApiResponseToAzureDocument
{
    private readonly List<LiveVacancy> liveVacanciesList = new List<LiveVacancy>()
        {
            new LiveVacancy()
            {
                VacancyId = Guid.NewGuid(),
                VacancyTitle = "Vacancy 1",
                ApprenticeshipTitle = "Apprenticeship title 1",
                Description = "<ul>\r\n<li>API Testing</li>\r\n<li>Selenium Training</li>\r\n<li>Automation Testing</li>\r\n</ul>",
                EmployerLocation = new Address
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
                Route = "one",
                Level = 4,
                Wage = new Wage
                {
                    Duration = 2,
                    DurationUnit = "Year",
                    WorkingWeekDescription = "Monday to Friday 9am to 5pm, shifts, may work evenings and weekends.",
                    WeeklyHours = 35,
                    WageType = "Competitive Salary",
                    WageAdditionalInformation = "all about bonuses",
                    FixedWageYearlyAmount = 25000
                }
            },
            new LiveVacancy()
            {
                VacancyId = Guid.NewGuid(),
                VacancyTitle = "Vacancy 2",
                ApprenticeshipTitle = "Apprenticeship title 2",
                Description = "<ul>\r\n<li>Advance Mathematics</li>\r\n<li>Computer Modelling</li>\r\n<li>Speech Recognition</li>\r\n</ul>\r\n<p>5day a week training</p>",
                EmployerLocation = new Address
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
                Route = "two",
                Level = 7,
                Wage = new Wage
                {
                    Duration = 12,
                    DurationUnit = "Month",
                    WorkingWeekDescription = "Monday to Friday 9am to 5pm, shifts, may work evenings and weekends.",
                    WeeklyHours = 35,
                    WageType = "Competitive Salary",
                    WageAdditionalInformation = "pay information about bonus",
                    FixedWageYearlyAmount = 25000
                }
            },
            new LiveVacancy()
            {
                VacancyId = Guid.NewGuid(),
                VacancyTitle = "Vacancy 3",
                ApprenticeshipTitle = "Apprenticeship title 3",
                Description = "<p>Write a few sentences about the apprenticeship to attract applicants. This information will appear under your advert title when applicants look through a list of adverts - make it stand out.</p>",
                EmployerLocation = new Address
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
                Route = "three",
                Level = 5,
                Wage = new Wage
                {
                    Duration = 1,
                    DurationUnit = "Year",
                    WorkingWeekDescription = "Monday to Friday 9am to 5pm, shifts, may work evenings and weekends.",
                    WeeklyHours = 35,
                    WageType = "Competitive Salary",
                    WageAdditionalInformation = "all about bonuses",
                    FixedWageYearlyAmount = 25000
                }
            }
        };

    [Test, MoqAutoData]
    public void Then_Fields_Are_Mapped()
    {
        var source = liveVacanciesList.First();

        var apprenticeAzureSearchDocument = (ApprenticeAzureSearchDocument)source;

        using (new AssertionScope())
        {
            apprenticeAzureSearchDocument.Description.Should().BeEquivalentTo(source.Description);
            apprenticeAzureSearchDocument.Route.Should().BeEquivalentTo(source.Route);
            apprenticeAzureSearchDocument.EmployerName.Should().BeEquivalentTo(source.EmployerName);
            apprenticeAzureSearchDocument.HoursPerWeek.Should().Be((long)source.Wage.WeeklyHours);
            apprenticeAzureSearchDocument.ProviderName.Should().BeEquivalentTo(source.ProviderName);
            apprenticeAzureSearchDocument.StartDate.Should().Be(source.StartDate);
            apprenticeAzureSearchDocument.PostedDate.Should().Be(source.LiveDate);
            apprenticeAzureSearchDocument.ClosingDate.Should().Be(source.ClosingDate);
            apprenticeAzureSearchDocument.Title.Should().BeEquivalentTo(source.VacancyTitle);
            apprenticeAzureSearchDocument.Ukprn.Should().Be(source.ProviderId);
            apprenticeAzureSearchDocument.VacancyReference.Should().Be($"VAC{source.VacancyId}");
            AssertWageIsMapped(apprenticeAzureSearchDocument, source);
            AssertCourseIsMapped(apprenticeAzureSearchDocument, source);
            AssertAddressIsMapped(apprenticeAzureSearchDocument, source);
            apprenticeAzureSearchDocument.Location.Should().NotBeNull();
        }
    }

    public void AssertWageIsMapped(ApprenticeAzureSearchDocument apprenticeAzureSearchDocument, LiveVacancy source)
    {
        apprenticeAzureSearchDocument.Wage.WageAdditionalInformation.Should().BeEquivalentTo(source.Wage.WageAdditionalInformation);
        apprenticeAzureSearchDocument.Wage.WageAmount.Should().Be((long)source.Wage.FixedWageYearlyAmount);
        apprenticeAzureSearchDocument.Wage.WageType.Should().BeEquivalentTo(source.Wage.WageType);
        apprenticeAzureSearchDocument.Wage.WorkingWeekDescription.Should().BeEquivalentTo(source.Wage.WorkingWeekDescription);
    }

    public void AssertCourseIsMapped(ApprenticeAzureSearchDocument apprenticeAzureSearchDocument, LiveVacancy source)
    {
        apprenticeAzureSearchDocument.Course.Level.Should().Be(source.Level);
        apprenticeAzureSearchDocument.Course.LarsCode.Should().Be((long)Convert.ToDouble(source.ProgrammeId));
        apprenticeAzureSearchDocument.Course.Title.Should().BeEquivalentTo(source.ApprenticeshipTitle);
    }

    public void AssertAddressIsMapped(ApprenticeAzureSearchDocument apprenticeAzureSearchDocument, LiveVacancy source)
    {
        apprenticeAzureSearchDocument.Address.AddressLine1.Should().BeEquivalentTo(source.EmployerLocation.AddressLine1);
        apprenticeAzureSearchDocument.Address.AddressLine2.Should().BeEquivalentTo(source.EmployerLocation.AddressLine2);
        apprenticeAzureSearchDocument.Address.AddressLine3.Should().BeEquivalentTo(source.EmployerLocation.AddressLine3);
        apprenticeAzureSearchDocument.Address.AddressLine4.Should().BeEquivalentTo(source.EmployerLocation.AddressLine4);
        apprenticeAzureSearchDocument.Address.Postcode.Should().BeEquivalentTo(source.EmployerLocation.Postcode);
    }
}
