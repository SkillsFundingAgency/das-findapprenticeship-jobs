using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Moq.Protected;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Slack;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Infrastructure.Slack;

internal class WhenPostingSlackMessage
{
    [Test, MoqAutoData]
    public async Task Then_The_Call_Is_Made_Correctly(
        IOptions<SlackConfiguration> configuration,
        Mock<HttpMessageHandler> handler,
        CancellationTokenSource cancellationTokenSource,
        SlackMessage slackMessage)
    {
        // arrange
        HttpRequestMessage? capturedRequest = null;
        handler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .Callback((HttpRequestMessage request, CancellationToken _) => { capturedRequest = request; })
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent("""{ "ok": true }""", new MediaTypeHeaderValue("application/json")) });
        
        var httpClient = new HttpClient(handler.Object);
        var sut = new SlackClient(configuration, httpClient);

        // act
        await sut.PostMessageAsync(slackMessage, cancellationTokenSource.Token);
        var msg = (capturedRequest?.Content as JsonContent)?.Value as SlackMessage;

        // assert
        handler.Protected().Verify<Task<HttpResponseMessage>>("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        capturedRequest.Should().NotBeNull();
        capturedRequest.RequestUri.Should().Be("https://slack.com/api/chat.postMessage");
        capturedRequest.Method.Should().Be(HttpMethod.Post);
        msg.Should().Be(slackMessage);
    }
    
    [Test]
    [MoqInlineAutoData(true, null)]
    [MoqInlineAutoData(false, "error_message")]
    public async Task Then_The_Slack_Result_Is_Returned(
        bool ok,
        string? errorMessage,
        IOptions<SlackConfiguration> configuration,
        Mock<HttpMessageHandler> handler,
        CancellationTokenSource cancellationTokenSource,
        SlackMessage slackMessage)
    {
        // arrange
        var responseText = JsonSerializer.Serialize(new PostMessageResponse(ok, errorMessage));
        handler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(responseText, new MediaTypeHeaderValue("application/json")) });
        
        var httpClient = new HttpClient(handler.Object);
        var sut = new SlackClient(configuration, httpClient);

        // act
        var result = await sut.PostMessageAsync(slackMessage, cancellationTokenSource.Token);

        // assert
        result.Should().NotBeNull();
        result.Ok.Should().Be(ok);
        result.Error.Should().Be(errorMessage);
    }
}