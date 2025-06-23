using Microsoft.Extensions.Options;
using SFA.DAS.FindApprenticeship.Jobs.Application.Indexing;
using SFA.DAS.FindApprenticeship.Jobs.Application.Services;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Slack;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Indexing;

public class WhenVerifyingSnapshots
{
    [Test]
    [MoqInlineAutoData(1000, 1000, 0)]
    [MoqInlineAutoData(1000, 501, 0)]
    [MoqInlineAutoData(1000, 500, 1)]
    [MoqInlineAutoData(1000, 499, 1)]
    [MoqInlineAutoData(1000, 0, 1)]
    [MoqInlineAutoData(0, 0, 1)]
    [MoqInlineAutoData(100, 100, 0)]
    [MoqInlineAutoData(0, 1000, 0)]
    [MoqInlineAutoData(400, 1000, 0)]
    public async Task Alert_Is_Raised_The_Correct_Number_Of_Times_For_Basic_Document_Counts(
        long oldCount,
        long newCount,
        int messageCount,
        Mock<FunctionEnvironment> environment,
        Mock<IOptions<IndexingAlertConfiguration>> alertConfig,
        Mock<ISlackClient> slackClient)
    {
        // arrange
        var oldStats = new IndexStatistics(oldCount);
        var newStats = new IndexStatistics(newCount);

        alertConfig.Setup(x => x.Value).Returns(new IndexingAlertConfiguration(50, ["channelId"]));
        var sut = new IndexingAlertsManager(alertConfig.Object, environment.Object, slackClient.Object, Mock.Of<ILogger<IndexingAlertsManager>>());
        
        // act
        await sut.VerifySnapshotsAsync(oldStats, newStats);

        // assert
        slackClient.Verify(x => x.PostMessageAsync(It.IsAny<SlackMessage>(), It.IsAny<CancellationToken>()), Times.Exactly(messageCount));
    }
    
    [Test, MoqAutoData]
    public async Task VerifySnapshotsAsync_Handles_Slack_Exceptions(
        [Frozen] Mock<ISlackClient> slackClient,
        IndexingAlertsManager sut)
    {
        // arrange
        var oldStats = new IndexStatistics(1000);
        var newStats = new IndexStatistics(0);
        slackClient.Setup(x => x.PostMessageAsync(It.IsAny<SlackMessage>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
        var action = async () => await sut.VerifySnapshotsAsync(oldStats, newStats);
        
        // act/assert
        await action.Should().NotThrowAsync();
    }
}