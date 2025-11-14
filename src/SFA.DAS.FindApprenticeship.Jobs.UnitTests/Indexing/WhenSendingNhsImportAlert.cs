using Microsoft.Extensions.Options;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Alerting;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Indexing;

public class WhenSendingNhsImportAlert
{
    [Test, MoqAutoData]
    public async Task SendNhsImportAlertAsync_Calls_The_Teams_Api(
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
        await sut.SendNhsImportAlertAsync();

        // assert
        teamsClient.Verify(x => x.PostMessageAsync(It.IsAny<AlertMessage>(), It.IsAny<CancellationToken>()), Times.Once);
        capturedMessage.Should().NotBeNull();
        capturedMessage.Origin.Should().Be("FAA Indexer");
        capturedMessage.Detail.Should().Be("No NHS vacancies were imported");
        capturedMessage.Environment.Should().Be(environment.EnvironmentName);
    }
    
    [Test, MoqAutoData]
    public async Task SendNhsImportAlertAsync_Handles_Teams_Exceptions(
        [Frozen] Mock<ITeamsClient> teamsClient,
        IndexingAlertsManager sut)
    {
        // arrange
        teamsClient.Setup(x => x.PostMessageAsync(It.IsAny<AlertMessage>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
        var action = async () => await sut.SendNhsImportAlertAsync();
        
        // act/assert
        await action.Should().NotThrowAsync();
        teamsClient.Verify(x => x.PostMessageAsync(It.IsAny<AlertMessage>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}