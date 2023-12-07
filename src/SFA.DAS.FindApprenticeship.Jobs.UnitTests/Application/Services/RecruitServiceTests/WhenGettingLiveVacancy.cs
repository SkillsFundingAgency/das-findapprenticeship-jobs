using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Application.Services;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Services.RecruitServiceTests;
public class WhenGettingLiveVacancy
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_And_A_LiveVacancy_Is_Returned(
    ApiResponse<GetLiveVacancyApiResponse> response,
    [Frozen] Mock<IRecruitApiClient> apiClient,
    RecruitService service)
    {
        apiClient.Setup(x =>
        x.Get<GetLiveVacancyApiResponse>(
            It.Is<GetLiveVacancyApiRequest>(c => c.GetUrl.Contains($"livevacancies/{response.Body.VacancyReference}"))))
            .ReturnsAsync(response);

        var actual = await service.GetLiveVacancy(response.Body.VacancyReference);

        actual.Should().BeEquivalentTo(response.Body);
    }
}
