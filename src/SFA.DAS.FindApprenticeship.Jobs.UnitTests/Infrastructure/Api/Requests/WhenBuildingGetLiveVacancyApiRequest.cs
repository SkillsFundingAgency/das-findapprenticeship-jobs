using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Infrastructure.Api.Requests;

[TestFixture]
public class WhenBuildingGetLiveVacancyApiRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Built(long vacancyReference)
    {
        var actual = new GetLiveVacancyApiRequest(vacancyReference);

        actual.GetUrl.Should().Be($"livevacancies/{vacancyReference}");
    }
}
