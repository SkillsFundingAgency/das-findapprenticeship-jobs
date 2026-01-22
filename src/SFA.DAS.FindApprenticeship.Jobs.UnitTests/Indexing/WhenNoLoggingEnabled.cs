using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Alerting;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Indexing;

public class WhenNoLoggingEnabled
{
    [Test, MoqAutoData]
    public async Task Then_An_Success_Response_Is_Returned(
        AlertMessage message,
        NoLoggingTeamsClient sut)
    {
        // act
        var result = await sut.PostMessageAsync(message, CancellationToken.None);

        // assert
        result.Ok.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}