using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Infrastructure.Api.Requests;
public class WhenBuildingGetLiveVacancyApiRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Built(Guid vacancyId)
    {
        var actual = new GetLiveVacancyApiRequest(vacancyId);

        actual.GetUrl.Should().Be($"livevacancy?vacancyId={vacancyId}");
    }
}
