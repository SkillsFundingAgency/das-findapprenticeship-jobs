using System.Net.Http.Json;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Alerting;

public record TeamsResponse(bool Ok, HttpStatusCode? StatusCode);

public interface ITeamsClient
{
    Task<TeamsResponse> PostMessageAsync(AlertMessage message, CancellationToken cancellationToken = default);
}

public class TeamsClient(IIndexingAlertingConfiguration configuration, HttpClient httpClient, ILogger<TeamsClient> logger): ITeamsClient
{
    public async Task<TeamsResponse> PostMessageAsync(AlertMessage message, CancellationToken cancellationToken = default)
    {
        if (!Uri.TryCreate(configuration.TeamsAlertWebhookUrl, UriKind.Absolute, out var uri))
        {
            logger.LogError("TeamsConfiguration TeamsAlertWebhookUrl is not set or invalid");
            return new TeamsResponse(true, HttpStatusCode.OK);
        }
            
        var request = new HttpRequestMessage()
        {
            RequestUri = uri,
            Method = HttpMethod.Post,
            Content = JsonContent.Create(message),
        };
        
        var response = await httpClient.SendAsync(request, cancellationToken);
        return new TeamsResponse(response.IsSuccessStatusCode, response.StatusCode);
    }
}

public class NoLoggingTeamsClient : ITeamsClient
{
    public Task<TeamsResponse> PostMessageAsync(AlertMessage message, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new TeamsResponse(true, HttpStatusCode.OK));
    }
}