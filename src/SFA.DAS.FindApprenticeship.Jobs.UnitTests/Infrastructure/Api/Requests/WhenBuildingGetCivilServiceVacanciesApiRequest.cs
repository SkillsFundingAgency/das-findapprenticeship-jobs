using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Infrastructure.Api.Requests;
[TestFixture]
public class WhenBuildingGetCivilServiceVacanciesApiRequest
{
    [Test, MoqAutoData]
    public void Then_The_GetUrl_Is_Built_Correctly()
    {
        var request = new GetCivilServiceVacanciesApiRequest();
        request.GetUrl.Should().Be("CivilServiceVacancies");
    }
}