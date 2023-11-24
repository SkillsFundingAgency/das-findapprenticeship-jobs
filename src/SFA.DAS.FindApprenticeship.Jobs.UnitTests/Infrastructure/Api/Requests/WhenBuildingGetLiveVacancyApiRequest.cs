using AutoFixture.NUnit3;
using NUnit.Framework;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Infrastructure.Api.Requests;
public class WhenBuildingGetLiveVacancyApiRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Build(long vacancyId)
    {
        var actual = new GetLiveVacancyRequest(vacancyId);

        actual.GetUrl.Should().Be($"livevacancy?vacancyId={vacancyId}");
    }
}
