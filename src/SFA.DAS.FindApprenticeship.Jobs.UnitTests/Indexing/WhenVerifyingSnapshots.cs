using SFA.DAS.FindApprenticeship.Jobs.Application.Services;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Alerting;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Indexing;

public class WhenVerifyingSnapshots
{
    [Test]
    [MoqInlineAutoData(50, 1000, 1000, 0)]
    [MoqInlineAutoData(50, 1000, 501, 0)]
    [MoqInlineAutoData(50, 1000, 500, 1)]
    [MoqInlineAutoData(50, 1000, 499, 1)]
    [MoqInlineAutoData(50, 1000, 0, 1)]
    [MoqInlineAutoData(50, 0, 0, 1)]
    [MoqInlineAutoData(50, 100, 100, 0)]
    [MoqInlineAutoData(50, 0, 1000, 0)]
    [MoqInlineAutoData(50, 400, 1000, 0)]
    [MoqInlineAutoData(75, 100, 26, 0)]
    [MoqInlineAutoData(75, 100, 25, 1)]
    public async Task Alert_Is_Raised_The_Correct_Number_Of_Times_For_Basic_Document_Counts(
        int threshold,
        long oldCount,
        long newCount,
        int messageCount,
        [Frozen] Mock<FunctionEnvironment> environment,
        [Frozen] Mock<IIndexingAlertingConfiguration> alertConfig,
        [Frozen] Mock<ITeamsClient> teamsClient,
        IndexingAlertsManager sut)
    {
        // arrange
        var oldStats = new IndexStatistics(oldCount);
        var newStats = new IndexStatistics(newCount);
        alertConfig.Setup(x => x.DocumentDecreasePercentageThreshold).Returns(threshold);
        
        // act
        await sut.VerifySnapshotsAsync(oldStats, newStats);

        // assert
        teamsClient.Verify(x => x.PostMessageAsync(It.IsAny<AlertMessage>(), It.IsAny<CancellationToken>()), Times.Exactly(messageCount));
    }
    
    [Test, MoqAutoData]
    public async Task VerifySnapshotsAsync_Handles_Slack_Exceptions(
        [Frozen] Mock<ITeamsClient> teamsClient,
        IndexingAlertsManager sut)
    {
        // arrange
        var oldStats = new IndexStatistics(1000);
        var newStats = new IndexStatistics(0);
        teamsClient.Setup(x => x.PostMessageAsync(It.IsAny<AlertMessage>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
        var action = async () => await sut.VerifySnapshotsAsync(oldStats, newStats);
        
        // act/assert
        await action.Should().NotThrowAsync();
    }
}