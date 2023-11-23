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
        [Test, MoqAutoData]
        public async Task Then_The_LiveVacancies_Are_Retrieved_And_Index_Is_Created(
            GetLiveVacanciesApiResponse liveVacanciesApiResponse,
            [Frozen] Mock<IRecruitService> recruitService,
            [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
            RecruitIndexerJobHandler sut)
        {
            liveVacanciesApiResponse.Vacancies = TestData.LiveVacancies;
            recruitService.Setup(x => x.GetLiveVacancies(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(liveVacanciesApiResponse);
            azureSearchHelper.Setup(x => x.CreateIndex(It.IsAny<string>())).Returns(Task.CompletedTask);
            azureSearchHelper.Setup(x => x.UploadDocuments(It.IsAny<string>(), It.IsAny<List<ApprenticeAzureSearchDocument>>())).Returns(Task.CompletedTask);

            await sut.Handle();

            using (new AssertionScope())
            {
                recruitService.Verify(x => x.GetLiveVacancies(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
                azureSearchHelper.Verify(x => x.CreateIndex(It.IsAny<string>()), Times.Once());
                azureSearchHelper.Verify(x => x.UploadDocuments(It.IsAny<string>(), It.IsAny<List<ApprenticeAzureSearchDocument>>()), Times.Once());
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
                azureSearchHelper.Verify(x => x.UploadDocuments(It.IsAny<string>(), It.IsAny<List<ApprenticeAzureSearchDocument>>()), Times.Never());
            }
        }
    }
}