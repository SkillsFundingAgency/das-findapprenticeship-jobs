using FluentAssertions.Execution;
using SFA.DAS.FindApprenticeship.Jobs.Application.Services;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Services.RecruitServiceTests
{
    [TestFixture]
    public class WhenGettingLiveNhsVacancies
    {

        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_Nhs_Live_Vacancies_Returned(
            int pageSize,
            int pageNo,
            ApiResponse<GetNhsLiveVacanciesApiResponse> response,
            [Frozen] Mock<IOuterApiClient> apiClient,
            FindApprenticeshipJobsService service)
        {
            apiClient.Setup(x =>
                    x.Get<GetNhsLiveVacanciesApiResponse>(
                        It.Is<GetNhsLiveVacanciesApiRequest>(c => c.GetUrl.Contains("nhsvacancies"))))
                .ReturnsAsync(response);

            var actual = await service.GetNhsLiveVacancies();

            using (new AssertionScope())
            {
                actual.Should().BeEquivalentTo(response.Body);
            }
        }
    }
}
