using FluentAssertions.Execution;
using SFA.DAS.FindApprenticeship.Jobs.Application.Services;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Services.RecruitServiceTests;
[TestFixture]
public class WhenGettingCivilServiceVacancies
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_And_CivilService_Live_Vacancies_Returned(
        int pageSize,
        int pageNo,
        ApiResponse<GetCivilServiceLiveVacanciesApiResponse> response,
        [Frozen] Mock<IOuterApiClient> apiClient,
        FindApprenticeshipJobsService service)
    {
        apiClient.Setup(x =>
                x.Get<GetCivilServiceLiveVacanciesApiResponse>(
                    It.Is<GetCivilServiceVacanciesApiRequest>(c => c.GetUrl.Contains("CivilServiceVacancies"))))
            .ReturnsAsync(response);

        var actual = await service.GetCivilServiceLiveVacancies();

        using (new AssertionScope())
        {
            actual.Should().BeEquivalentTo(response.Body);
        }
    }
}