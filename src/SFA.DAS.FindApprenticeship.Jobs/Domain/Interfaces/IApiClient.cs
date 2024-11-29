namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
public interface IApiClient
{
    Task<ApiResponse<TResponse>> Get<TResponse>(IGetApiRequest request);

    Task<ApiResponse<TResponse>> Post<TResponse>(IPostApiRequest request);

    Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequestWithData request);
}
