using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
public class AzureSearchApiClient : ApiClientBase, IAzureSearchApiClient
{
    private readonly IAzureClientCredentialHelper _azureClientCredentialHelper;
    private readonly FindApprenticeshipJobsConfiguration _configuration;

    public AzureSearchApiClient(HttpClient httpClient, IOptions<FindApprenticeshipJobsConfiguration> configuration, IAzureClientCredentialHelper azureClientCredentialHelper) : base(httpClient)
    {
        _azureClientCredentialHelper = azureClientCredentialHelper;
        _configuration = configuration.Value;
        httpClient.BaseAddress = new Uri(configuration.Value.AzureSearchBaseUrl);
    }

    protected override async Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
    {
        //TODO: Add Managed Identity rather than API Key
        var token = await _azureClientCredentialHelper.GetAccessTokenAsync(_configuration.AzureSearchResource);
        httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        httpRequestMessage.Headers.Add("api-key", _configuration.AzureSearchKey);
    }
}
