using Microsoft.Extensions.Options;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
public class OuterApiClient : ApiClientBase, IOuterApiClient
{
    private readonly FindApprenticeshipJobsConfiguration _configuration;

    public OuterApiClient(HttpClient httpClient, IOptions<FindApprenticeshipJobsConfiguration> configuration) : base(httpClient)
    {
        _configuration = configuration.Value;
        httpClient.BaseAddress = new System.Uri(configuration.Value.ApimBaseUrl);
    }

    protected override Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
    {
        httpRequestMessage.Headers.Add("Ocp-Apim-Subscription-Key", _configuration.ApimKey);
        httpRequestMessage.Headers.Add("X-Version", "1");
        return Task.CompletedTask;
    }
}