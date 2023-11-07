using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using System;
using System.Net;
using Newtonsoft.Json;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
public class ApiClient : IApiClient
{
    private readonly HttpClient _client;
    private readonly FindApprenticeshipJobsApiConfiguration _configuration;

    public ApiClient(HttpClient client, IOptions<FindApprenticeshipJobsApiConfiguration> configuration)
    {
        _client = client;
        _configuration = configuration.Value;
        _client.BaseAddress = new Uri(_configuration.BaseUrl);
    }

    public async Task<TResponse> Get<TResponse>(IGetApiRequest request)
    {
        AddHeaders();

        var response = await _client.GetAsync(request.GetUrl).ConfigureAwait(false);

        if (response.StatusCode.Equals(HttpStatusCode.NotFound))
        {
            return default;
        }

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<TResponse>(json);
        }

        response.EnsureSuccessStatusCode();

        return default;
    }

    private void AddHeaders()
    {
        _client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _configuration.Key);
        _client.DefaultRequestHeaders.Add("X-Version", "1");
    }
}
