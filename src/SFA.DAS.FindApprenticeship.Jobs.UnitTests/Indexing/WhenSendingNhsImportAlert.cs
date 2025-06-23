using Microsoft.Extensions.Options;
using SFA.DAS.FindApprenticeship.Jobs.Application.Indexing;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Slack;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Indexing;

public class WhenSendingNhsImportAlert
{
    [Test, MoqAutoData]
    public async Task SendNhsImportAlertAsync_Calls_The_Slack_Api(
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
        await sut.SendNhsImportAlertAsync();
        var block = capturedMessage?.Blocks?.Last() as SectionBlock;

        // assert
        slackClient.Verify(x => x.PostMessageAsync(It.IsAny<SlackMessage>(), It.IsAny<CancellationToken>()), Times.Exactly(alertConfig.Value.Channels!.Count));
        block.Should().NotBeNull();
        block.Text.Text.Should().Contain("no NHS vacancies were imported");
    }
    
    [Test, MoqAutoData]
    public async Task SendNhsImportAlertAsync_Handles_Slack_Exceptions(
        [Frozen] Mock<ISlackClient> slackClient,
        IndexingAlertsManager sut)
    {
        // arrange
        slackClient.Setup(x => x.PostMessageAsync(It.IsAny<SlackMessage>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
        var action = async () => await sut.SendNhsImportAlertAsync();
        
        // act/assert
        await action.Should().NotThrowAsync();
        slackClient.Verify(x => x.PostMessageAsync(It.IsAny<SlackMessage>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}