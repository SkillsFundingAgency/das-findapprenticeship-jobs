using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using System.Text.Json;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
public abstract class ApiClientBase
{
    private readonly HttpClient _httpClient;

    protected ApiClientBase(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse<TResponse>> Get<TResponse>(IGetApiRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, request.GetUrl);
        await AddAuthenticationHeader(requestMessage);

        var response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);

        return await ProcessResponse<TResponse>(response);
    }
    public async Task<ApiResponse<TResponse>> Post<TResponse>(IPostApiRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, request.PostUrl);
        await AddAuthenticationHeader(requestMessage);

        var response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);

        return await ProcessResponse<TResponse>(response);
    }

    public async Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequestWithData request)
    {
        var stringContent = new StringContent(JsonSerializer.Serialize(request.Data), System.Text.Encoding.UTF8, "application/json");

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, request.PostUrl);
        requestMessage.Content = stringContent;
        await AddAuthenticationHeader(requestMessage);

        var response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);

        return await ProcessResponse<TResponse>(response);
    }

    protected abstract Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage);

    private static async Task<ApiResponse<TResponse>> ProcessResponse<TResponse>(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        var errorContent = "";
        var responseBody = (TResponse)default;

        if (!response.IsSuccessStatusCode)
        {
            errorContent = json;
        }
        else if (typeof(TResponse) != typeof(NullResponse))
        {
            responseBody = JsonSerializer.Deserialize<TResponse>
                (json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        var apiResponse = new ApiResponse<TResponse>(responseBody, response.StatusCode, errorContent);

        return apiResponse;
    }
}