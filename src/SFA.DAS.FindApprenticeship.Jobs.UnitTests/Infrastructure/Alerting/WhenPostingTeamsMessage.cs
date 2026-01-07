using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Moq.Protected;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Alerting;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Infrastructure.Alerting;

internal class WhenPostingTeamsMessage
{
    [Test, MoqAutoData]
    public async Task Then_The_Call_Is_Made_Correctly(
        Mock<HttpMessageHandler> handler,
        CancellationTokenSource cancellationTokenSource,
        AlertMessage alertMessage)
    {
        // arrange
        HttpRequestMessage? capturedRequest = null;
        handler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .Callback((HttpRequestMessage request, CancellationToken _) => { capturedRequest = request; })
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent("", new MediaTypeHeaderValue("application/json")) });
        var configuration = new IndexingAlertingConfiguration { TeamsAlertWebhookUrl = "https://localhost" };
        
        var httpClient = new HttpClient(handler.Object);
        var sut = new TeamsClient(configuration, httpClient, Mock.Of<ILogger<TeamsClient>>());

        // act
        await sut.PostMessageAsync(alertMessage, cancellationTokenSource.Token);
        var msg = (capturedRequest?.Content as JsonContent)?.Value as AlertMessage;

        // assert
        handler.Protected().Verify<Task<HttpResponseMessage>>("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        capturedRequest.Should().NotBeNull();
        capturedRequest.RequestUri.Should().Be(configuration.TeamsAlertWebhookUrl);
        capturedRequest.Method.Should().Be(HttpMethod.Post);
        msg.Should().Be(alertMessage);
    }
}