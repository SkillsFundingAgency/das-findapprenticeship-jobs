using AutoFixture.NUnit3;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Services.Handlers
{
    public class WhenHandlingRecruitIndexerJob
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
                    WageAdditionalInformation = "all about bonuses"
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
                    WageAdditionalInformation = "all about bonuses"
                }
            }
        };

        [Test, MoqAutoData]
        public async Task Then_The_LiveVacancies_Are_Retrieved_And_Index_Is_Created(
            GetLiveVacanciesApiResponse liveVacanciesApiResponse,
            [Frozen] Mock<IRecruitService> recruitService,
            [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
            RecruitIndexerJobHandler sut)
        {
            liveVacanciesApiResponse.Vacancies = liveVacanciesList;
            recruitService.Setup(x => x.GetLiveVacancies(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(liveVacanciesApiResponse);
            azureSearchHelper.Setup(x => x.DeleteIndex(It.IsAny<string>())).Returns(Task.CompletedTask);
            azureSearchHelper.Setup(x => x.CreateIndex(It.IsAny<string>())).Returns(Task.CompletedTask);
            azureSearchHelper.Setup(x => x.UploadDocuments(It.IsAny<List<ApprenticeAzureSearchDocument>>())).Returns(Task.CompletedTask);

            await sut.Handle();

            using (new AssertionScope())
            {
                recruitService.Verify(x => x.GetLiveVacancies(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
                azureSearchHelper.Verify(x => x.DeleteIndex(It.IsAny<string>()), Times.Once());
                azureSearchHelper.Verify(x => x.CreateIndex(It.IsAny<string>()), Times.Once());
                azureSearchHelper.Verify(x => x.UploadDocuments(It.IsAny<List<ApprenticeAzureSearchDocument>>()), Times.Once());
            }
        }

        [Test, MoqAutoData]
        public async Task Then_LiveVacancies_Is_Null_And_Index_Is_Not_Created(
            [Frozen] Mock<IRecruitService> recruitService,
            [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
            RecruitIndexerJobHandler sut)
        {
            recruitService.Setup(x => x.GetLiveVacancies(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(() => null);

            await sut.Handle();

            using (new AssertionScope())
            {
                azureSearchHelper.Verify(x => x.DeleteIndex(It.IsAny<string>()), Times.Never());
                azureSearchHelper.Verify(x => x.CreateIndex(It.IsAny<string>()), Times.Never());
                azureSearchHelper.Verify(x => x.UploadDocuments(It.IsAny<List<ApprenticeAzureSearchDocument>>()), Times.Never());
            }
        }
    }
}