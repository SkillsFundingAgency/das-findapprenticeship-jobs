using AutoFixture.NUnit3;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Handlers
{
    public class WhenHandlingRecruitIndexerJob
    {
        [Test, MoqAutoData]
        public async Task Then_The_LiveVacancies_Are_Retrieved_And_Index_Is_Created(
            List<LiveVacancy> liveVacancies,
            List<GetNhsLiveVacanciesApiResponse.NhsLiveVacancy> nhsLiveVacancies,
            [Frozen] Mock<IFindApprenticeshipJobsService> findApprenticeshipJobsService,
            [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
            [Frozen] Mock<IDateTimeService> dateTimeService,
            DateTime currentDateTime,
            RecruitIndexerJobHandler sut)
        {
            liveVacancies.RemoveRange(1, 2);
            liveVacancies.FirstOrDefault().OtherAddresses = new List<Address>
            {
                new Address
                {
                    AddressLine1 = "124 Bards" 
                },
                new Address
                {
                    AddressLine1 = "29 bfenman"
                }
            };

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

            findApprenticeshipJobsService.Setup(x => x.GetLiveVacancies(It.IsAny<int>(), It.IsAny<int>(), null)).ReturnsAsync(liveVacanciesApiResponse);
            findApprenticeshipJobsService.Setup(x => x.GetNhsLiveVacancies()).ReturnsAsync(nhsLiveVacanciesApiResponse);
            azureSearchHelper.Setup(x => x.CreateIndex(It.IsAny<string>())).Returns(Task.CompletedTask);
            azureSearchHelper.Setup(x => x.UploadDocuments(It.IsAny<string>(), It.IsAny<List<ApprenticeAzureSearchDocument>>())).Returns(Task.CompletedTask);
            azureSearchHelper.Setup(x => x.UpdateAlias(Constants.AliasName, expectedIndexName)).Returns(Task.CompletedTask);

            await sut.Handle();

            using (new AssertionScope())
            {
                findApprenticeshipJobsService.Verify(x => x.GetLiveVacancies(It.IsAny<int>(), It.IsAny<int>(), null), Times.Exactly(liveVacanciesApiResponse.TotalPages));
                findApprenticeshipJobsService.Verify(x => x.GetNhsLiveVacancies(), Times.Exactly(nhsLiveVacanciesApiResponse.TotalPages));
                azureSearchHelper.Verify(x => x.CreateIndex(expectedIndexName), Times.Once());
                azureSearchHelper.Verify(x => x.UploadDocuments(expectedIndexName, It.IsAny<List<ApprenticeAzureSearchDocument>>()), Times.Exactly(liveVacanciesApiResponse.TotalPages));
                azureSearchHelper.Verify(x => x.UpdateAlias(Constants.AliasName, expectedIndexName), Times.Once);
            }
        }

        [Test, MoqAutoData]
        public async Task Then_LiveVacancies_Is_Null_And_Index_Is_Not_Created(
            [Frozen] Mock<IFindApprenticeshipJobsService> findApprenticeshipJobsService,
            [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
            RecruitIndexerJobHandler sut)
        {
            findApprenticeshipJobsService.Setup(x => x.GetLiveVacancies(It.IsAny<int>(), It.IsAny<int>(), null)).ReturnsAsync(() => null);
            findApprenticeshipJobsService.Setup(x => x.GetNhsLiveVacancies()).ReturnsAsync(() => null);

            await sut.Handle();

            using (new AssertionScope())
            {
                azureSearchHelper.Verify(x => x.CreateIndex(It.IsAny<string>()), Times.Once());
                azureSearchHelper.Verify(x => x.UploadDocuments(It.IsAny<string>(), It.IsAny<List<ApprenticeAzureSearchDocument>>()), Times.Never());
                azureSearchHelper.Verify(x => x.UpdateAlias(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            }
        }
    }
}