using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Infrastructure.Api.Requests;
public class WhenBuildingGetLiveVacanciesRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Built(int pageNo, int pageSize)
    {
        var actual = new GetLiveVacanciesApiRequest(pageNo, pageSize);

        actual.GetUrl.Should().Be($"livevacancies?pageSize={pageSize}&pageNo={pageNo}");
    }
}
