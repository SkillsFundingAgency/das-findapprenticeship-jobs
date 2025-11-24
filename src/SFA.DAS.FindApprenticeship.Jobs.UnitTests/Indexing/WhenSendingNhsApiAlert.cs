using Microsoft.Extensions.Options;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Alerting;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Indexing;

public class WhenSendingNhsApiAlert
{
    [Test, MoqAutoData]
    public async Task SendNhsApiAlertAsync_Calls_The_Teams_Api(
        [Frozen] IOptions<IndexingAlertingConfiguration> alertConfig,
        [Frozen] Mock<ITeamsClient> teamsClient,
        [Frozen] FunctionEnvironment environment,
        IndexingAlertsManager sut)
    {
        // arrange
        AlertMessage? capturedMessage = null;
        teamsClient
            .Setup(x => x.PostMessageAsync(It.IsAny<AlertMessage>(), It.IsAny<CancellationToken>()))
            .Callback((AlertMessage message, CancellationToken _) => { capturedMessage = message; });

        // act
        await sut.SendNhsApiAlertAsync();

        // assert
        teamsClient.Verify(x => x.PostMessageAsync(It.IsAny<AlertMessage>(), It.IsAny<CancellationToken>()), Times.Once);
        capturedMessage.Should().NotBeNull();
        capturedMessage.Origin.Should().Be("FAA Indexer");
        capturedMessage.Detail.Should().Be("The external NHS API returned no vacancies");
        capturedMessage.Environment.Should().Be(environment.EnvironmentName);
    }
    
    [Test, MoqAutoData]
    public async Task SendNhsApiAlertAsync_Handles_Teams_Exceptions(
        [Frozen] Mock<ITeamsClient> teamsClient,
        IndexingAlertsManager sut)
    {
        // arrange
        teamsClient.Setup(x => x.PostMessageAsync(It.IsAny<AlertMessage>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
        var action = async () => await sut.SendNhsApiAlertAsync();
        
        // act/assert
        await action.Should().NotThrowAsync();
        teamsClient.Verify(x => x.PostMessageAsync(It.IsAny<AlertMessage>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}