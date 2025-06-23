using Microsoft.Extensions.Options;
using SFA.DAS.FindApprenticeship.Jobs.Application.Indexing;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Slack;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Indexing;

public class WhenSendingNhsApiAlert
{
    [Test, MoqAutoData]
    public async Task SendNhsApiAlertAsync_Calls_The_Slack_Api(
        [Frozen] IOptions<IndexingAlertConfiguration> alertConfig,
        [Frozen] Mock<ISlackClient> slackClient,
        IndexingAlertsManager sut)
    {
        // arrange
        SlackMessage? capturedMessage = null;
        slackClient
            .Setup(x => x.PostMessageAsync(It.IsAny<SlackMessage>(), It.IsAny<CancellationToken>()))
            .Callback((SlackMessage message, CancellationToken _) => { capturedMessage = message; });

        // act
        await sut.SendNhsApiAlertAsync();
        var block = capturedMessage?.Blocks?.Last() as SectionBlock;

        // assert
        slackClient.Verify(x => x.PostMessageAsync(It.IsAny<SlackMessage>(), It.IsAny<CancellationToken>()), Times.Exactly(alertConfig.Value.Channels!.Count));
        block.Should().NotBeNull();
        block.Text.Text.Should().Contain("the external NHS API returned no vacancies");
    }
    
    [Test, MoqAutoData]
    public async Task SendNhsApiAlertAsync_Handles_Slack_Exceptions(
        [Frozen] Mock<ISlackClient> slackClient,
        IndexingAlertsManager sut)
    {
        // arrange
        slackClient.Setup(x => x.PostMessageAsync(It.IsAny<SlackMessage>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
        var action = async () => await sut.SendNhsApiAlertAsync();
        
        // act/assert
        await action.Should().NotThrowAsync();
        slackClient.Verify(x => x.PostMessageAsync(It.IsAny<SlackMessage>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}