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
        GetLiveVacancyApiResponse response,
        [Frozen] Mock<IOuterApiClient> apiClient,
        FindApprenticeshipJobsService service)
    {
        response.VacancyReference = vacancyReference.ToString();
        apiClient.Setup(x => x.Get<GetLiveVacancyApiResponse>(
            It.IsAny<GetLiveVacancyApiRequest>()))
            .ReturnsAsync(new ApiResponse<GetLiveVacancyApiResponse>(response, HttpStatusCode.OK, string.Empty));

        var actual = await service.GetLiveVacancy(vacancyReference);

        actual.Should().BeEquivalentTo(response);
    }

    [Test, MoqAutoData]
    public async Task Then_A_NotFound_Response_Returns_Null(
        VacancyReference vacancyReference,
        [Frozen] Mock<IOuterApiClient> apiClient,
        FindApprenticeshipJobsService service)
    {
        // Arrange
        apiClient.Setup(x => x.Get<GetLiveVacancyApiResponse>(
                It.IsAny<GetLiveVacancyApiRequest>()))
            .ReturnsAsync(new ApiResponse<GetLiveVacancyApiResponse>(null!, HttpStatusCode.NotFound, string.Empty));

        // Act
        var actual = await service.GetLiveVacancy(vacancyReference);

        // Assert
        actual.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_A_Success_Response_With_No_Body_Returns_Null(
        VacancyReference vacancyReference,
        [Frozen] Mock<IOuterApiClient> apiClient,
        FindApprenticeshipJobsService service)
    {
        // Arrange
        apiClient.Setup(x => x.Get<GetLiveVacancyApiResponse>(
                It.IsAny<GetLiveVacancyApiRequest>()))
            .ReturnsAsync(new ApiResponse<GetLiveVacancyApiResponse>(null!, HttpStatusCode.OK, string.Empty));

        // Act
        var actual = await service.GetLiveVacancy(vacancyReference);

        // Assert
        actual.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_An_Unsuccessful_Response_Throws(
        VacancyReference vacancyReference,
        [Frozen] Mock<IOuterApiClient> apiClient,
        FindApprenticeshipJobsService service)
    {
        // Arrange
        apiClient.Setup(x => x.Get<GetLiveVacancyApiResponse>(
                It.IsAny<GetLiveVacancyApiRequest>()))
            .ReturnsAsync(new ApiResponse<GetLiveVacancyApiResponse>(null!, HttpStatusCode.ServiceUnavailable, "service unavailable"));

        // Act
        Func<Task> act = () => service.GetLiveVacancy(vacancyReference);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }
}
