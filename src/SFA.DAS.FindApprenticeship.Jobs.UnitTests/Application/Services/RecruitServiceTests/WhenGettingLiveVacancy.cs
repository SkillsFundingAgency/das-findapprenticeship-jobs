using SFA.DAS.Common.Domain.Models;
using SFA.DAS.FindApprenticeship.Jobs.Application.Services;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Services.RecruitServiceTests;
public class WhenGettingLiveVacancy
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_And_A_LiveVacancy_Is_Returned(
        VacancyReference vacancyReference,
        ApiResponse<GetLiveVacancyApiResponse> response,
        [Frozen] Mock<IOuterApiClient> apiClient,
        FindApprenticeshipJobsService service)
    {
        response.Body.VacancyReference = vacancyReference.ToString();
        apiClient.Setup(x => x.Get<GetLiveVacancyApiResponse>(
            It.IsAny<GetLiveVacancyApiRequest>()))
            .ReturnsAsync(response);

        var actual = await service.GetLiveVacancy(vacancyReference);

        actual.Should().BeEquivalentTo(response.Body);
    }
}