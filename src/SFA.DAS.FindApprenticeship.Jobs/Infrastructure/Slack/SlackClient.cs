using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Slack;

public interface ISlackClient
{
    Task<PostMessageResponse> PostMessageAsync(SlackMessage message, CancellationToken cancellationToken = default);
}

public record struct PostMessageResponse(bool Ok, string? Error);

internal class SlackClient : ISlackClient
{
    private readonly HttpClient _httpClient;
    private const string PostMessageUrl = "https://slack.com/api/chat.postMessage";
    
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public SlackClient(IOptions<SlackConfiguration> slackConfiguration, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", slackConfiguration.Value.BotUserOAuthToken);
    }

    public async Task<PostMessageResponse> PostMessageAsync(SlackMessage message, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(PostMessageUrl, message, _jsonSerializerOptions, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PostMessageResponse>(cancellationToken);
    }
}