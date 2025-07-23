using FluentAssertions.Execution;
using SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Handlers
{
    public class WhenHandlingRecruitIndexerJob
    {
        [TestCase("\"OneLocation\"", AvailableWhere.OneLocation)]
        [TestCase("\"MultipleLocations\"", AvailableWhere.MultipleLocations)]
        [TestCase("\"AcrossEngland\"", AvailableWhere.AcrossEngland)]
        [TestCase("null", null)]
        public void Then_EmploymentLocationOption_Can_Be_Deserialized(string? value, AvailableWhere? expected)
        {
            // arrange
            var json = $"{{\"EmploymentLocationOption\":{value}}}";
            
            // act
            var newLiveVacancy = System.Text.Json.JsonSerializer.Deserialize<LiveVacancy>(json);

            // assert
            newLiveVacancy.Should().NotBeNull();
            newLiveVacancy!.EmploymentLocationOption.Should().Be(expected);
        }
        
        [Test, MoqAutoData]
        public async Task Then_The_LiveVacancies_Are_Retrieved_And_Index_Is_Created(
            List<LiveVacancy> liveVacancies,
            List<ExternalLiveVacancy> nhsLiveVacancies,
            List<ExternalLiveVacancy> civilServiceLiveVacancies,
            [Frozen] Mock<IFindApprenticeshipJobsService> findApprenticeshipJobsService,
            [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
            [Frozen] Mock<IDateTimeService> dateTimeService,
            DateTime currentDateTime,
            RecruitIndexerJobHandler sut)
        {
            dateTimeService.Setup(x => x.GetCurrentDateTime()).Returns(currentDateTime);

            var expectedIndexName = $"{Constants.IndexPrefix}{currentDateTime.ToString(Constants.IndexDateSuffixFormat)}";

            var liveVacanciesApiResponse = new GetLiveVacanciesApiResponse
            {
                Vacancies = liveVacancies,
                PageNo = 1,
                PageSize = liveVacancies.Count,
                TotalLiveVacancies = liveVacancies.Count,
                TotalLiveVacanciesReturned = liveVacancies.Count,
                TotalPages = 1
            };

            var nhsLiveVacanciesApiResponse = new GetNhsLiveVacanciesApiResponse
            {
                Vacancies = nhsLiveVacancies,
                PageNo = 1,
                PageSize = liveVacancies.Count,
                TotalLiveVacancies = liveVacancies.Count,
                TotalLiveVacanciesReturned = liveVacancies.Count,
                TotalPages = 1
            };

            var civilServiceLiveVacanciesApiResponse = new GetCivilServiceLiveVacanciesApiResponse()
            {
                Vacancies = civilServiceLiveVacancies,
                PageNo = 1,
                PageSize = liveVacancies.Count,
                TotalLiveVacancies = liveVacancies.Count,
                TotalLiveVacanciesReturned = liveVacancies.Count,
                TotalPages = 1
            };

            findApprenticeshipJobsService.Setup(x => x.GetLiveVacancies(It.IsAny<int>(), 500, null)).ReturnsAsync(liveVacanciesApiResponse);
            findApprenticeshipJobsService.Setup(x => x.GetNhsLiveVacancies()).ReturnsAsync(nhsLiveVacanciesApiResponse);
            findApprenticeshipJobsService.Setup(x => x.GetCivilServiceLiveVacancies()).ReturnsAsync(civilServiceLiveVacanciesApiResponse);
            azureSearchHelper.Setup(x => x.CreateIndex(It.IsAny<string>())).Returns(Task.CompletedTask);
            azureSearchHelper.Setup(x => x.UploadDocuments(It.IsAny<string>(), It.IsAny<List<ApprenticeAzureSearchDocument>>())).Returns(Task.CompletedTask);
            azureSearchHelper.Setup(x => x.UpdateAlias(Constants.AliasName, expectedIndexName)).Returns(Task.CompletedTask);

            await sut.Handle();

            using (new AssertionScope())
            {
                findApprenticeshipJobsService.Verify(x => x.GetLiveVacancies(It.IsAny<int>(), 500, null), Times.Exactly(liveVacanciesApiResponse.TotalPages));
                findApprenticeshipJobsService.Verify(x => x.GetNhsLiveVacancies(), Times.Exactly(nhsLiveVacanciesApiResponse.TotalPages));
                findApprenticeshipJobsService.Verify(x => x.GetCivilServiceLiveVacancies(), Times.Exactly(1));
                azureSearchHelper.Verify(x => x.CreateIndex(expectedIndexName), Times.Once());
                azureSearchHelper.Verify(x => x.UploadDocuments(expectedIndexName, It.IsAny<List<ApprenticeAzureSearchDocument>>()), Times.Exactly(3));
                azureSearchHelper.Verify(x => x.UpdateAlias(Constants.AliasName, expectedIndexName), Times.Once);
            }
        }

        [Test, MoqAutoData]
        public async Task Then_LiveVacancies_Is_Null_And_Index_Is_Not_Created(
            [Frozen] Mock<IFindApprenticeshipJobsService> findApprenticeshipJobsService,
            [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
            RecruitIndexerJobHandler sut)
        {
            findApprenticeshipJobsService.Setup(x => x.GetLiveVacancies(It.IsAny<int>(), 500, null)).ReturnsAsync(() => null);
            findApprenticeshipJobsService.Setup(x => x.GetNhsLiveVacancies()).ReturnsAsync(() => null);
            findApprenticeshipJobsService.Setup(x => x.GetCivilServiceLiveVacancies()).ReturnsAsync(() => null);

            await sut.Handle();

            using (new AssertionScope())
            {
                azureSearchHelper.Verify(x => x.CreateIndex(It.IsAny<string>()), Times.Once());
                azureSearchHelper.Verify(x => x.UploadDocuments(It.IsAny<string>(), It.IsAny<List<ApprenticeAzureSearchDocument>>()), Times.Never());
                azureSearchHelper.Verify(x => x.UpdateAlias(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            }
        }


        [Test, MoqAutoData]
        public async Task Handle_Should_Add_NhsLiveVacancies_With_EnglandOnly_Address_To_BatchDocuments(
            string countryName,
            List<LiveVacancy> liveVacancies,
            ExternalLiveVacancy nhsLiveVacancy,
            [Frozen] Mock<IFindApprenticeshipJobsService> findApprenticeshipJobsService,
            [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
            [Frozen] Mock<IDateTimeService> dateTimeService,
            DateTime currentDateTime,
            RecruitIndexerJobHandler sut)
        {
            nhsLiveVacancy.Address!.Country = countryName;

            dateTimeService.Setup(x => x.GetCurrentDateTime()).Returns(currentDateTime);

            var expectedIndexName = $"{Constants.IndexPrefix}{currentDateTime.ToString(Constants.IndexDateSuffixFormat)}";

            var liveVacanciesApiResponse = new GetLiveVacanciesApiResponse
            {
                Vacancies = [],
                PageNo = 1,
                PageSize = 0,
                TotalLiveVacancies = 0,
                TotalLiveVacanciesReturned = 0,
                TotalPages = 1
            };

            var nhsLiveVacanciesApiResponse = new GetNhsLiveVacanciesApiResponse
            {
                Vacancies = new List<ExternalLiveVacancy> {nhsLiveVacancy},
                PageNo = 1,
                PageSize = liveVacancies.Count,
                TotalLiveVacancies = liveVacancies.Count,
                TotalLiveVacanciesReturned = liveVacancies.Count,
                TotalPages = 1
            };

            var civilServiceLiveVacanciesApiResponse = new GetCivilServiceLiveVacanciesApiResponse()
            {
                Vacancies = [],
                PageNo = 1,
                PageSize = liveVacancies.Count,
                TotalLiveVacancies = liveVacancies.Count,
                TotalLiveVacanciesReturned = liveVacancies.Count,
                TotalPages = 1
            };

            findApprenticeshipJobsService.Setup(x => x.GetLiveVacancies(It.IsAny<int>(), It.IsAny<int>(), null)).ReturnsAsync(liveVacanciesApiResponse);
            findApprenticeshipJobsService.Setup(x => x.GetNhsLiveVacancies()).ReturnsAsync(nhsLiveVacanciesApiResponse);
            findApprenticeshipJobsService.Setup(x => x.GetCivilServiceLiveVacancies()).ReturnsAsync(civilServiceLiveVacanciesApiResponse);
            azureSearchHelper.Setup(x => x.CreateIndex(It.IsAny<string>())).Returns(Task.CompletedTask);
            azureSearchHelper.Setup(x => x.UploadDocuments(It.IsAny<string>(), It.IsAny<List<ApprenticeAzureSearchDocument>>())).Returns(Task.CompletedTask);
            azureSearchHelper.Setup(x => x.UpdateAlias(Constants.AliasName, expectedIndexName)).Returns(Task.CompletedTask);

            // Act
            await sut.Handle();

            // Assert
            azureSearchHelper.Verify(s => s.UploadDocuments(It.IsAny<string>(),
                It.Is<IEnumerable<ApprenticeAzureSearchDocument>>(docs => docs.Any(d => d.Address!.Country == Constants.EnglandOnly))), Times.Never);
        }
    }
}