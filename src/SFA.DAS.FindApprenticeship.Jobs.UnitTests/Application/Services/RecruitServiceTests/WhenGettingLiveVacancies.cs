using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
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
        int pageSize,
        int pageNo,
        ApiResponse<GetLiveVacanciesApiResponse> response,
        [Frozen] Mock<IRecruitApiClient> apiClient,
        RecruitService service)
    {
        response.Body.PageNo = pageNo;
        response.Body.PageSize = pageSize;
        apiClient.Setup(x =>
        x.Get<GetLiveVacanciesApiResponse>(
            It.Is<GetLiveVacanciesRequest>(c => c.GetUrl.Contains($"livevacancies?pageSize={pageSize}&pageNo={pageNo}"))))
            .ReturnsAsync(response);

        var actual = await service.GetLiveVacancies(pageNo, pageSize);

        using (new AssertionScope())
        {
            actual.Should().BeEquivalentTo(response.Body);
            actual.PageSize.Should().Be(pageSize);
            actual.PageNo.Should().Be(pageNo);
        }
    }
}
