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
public class WhenGettingLiveVacancies
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_And_Live_Vacancies_Returned(
        ApiResponse<GetLiveVacanciesApiResponse> response,
        [Frozen] Mock<IRecruitApiClient> apiClient,
        RecruitService service)
    {
        apiClient.Setup(x =>
        x.Get<GetLiveVacanciesApiResponse>(
            It.Is<GetLiveVacanciesRequest>(c => c.GetUrl.Contains($"livevacancies?pageSize={It.IsAny<int>()}&pageNo={It.IsAny<int>()}"))))
            .ReturnsAsync(response);

        var actual = await service.GetLiveVacancies(It.IsAny<int>(), It.IsAny<int>());

        actual.Should().BeEquivalentTo(response.Body);
    }
}
