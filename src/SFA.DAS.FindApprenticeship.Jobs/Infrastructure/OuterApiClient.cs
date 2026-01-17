using Microsoft.Extensions.Options;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
public class OuterApiClient(HttpClient httpClient, IOptions<FindApprenticeshipJobsConfiguration> configuration) : ApiClientBase(httpClient), IOuterApiClient
{
    private readonly FindApprenticeshipJobsConfiguration _configuration = configuration.Value;

    protected override Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
    {
        httpRequestMessage.Headers.Add("Ocp-Apim-Subscription-Key", _configuration.ApimKey);
        httpRequestMessage.Headers.Add("X-Version", "1");
        return Task.CompletedTask;
    }
}