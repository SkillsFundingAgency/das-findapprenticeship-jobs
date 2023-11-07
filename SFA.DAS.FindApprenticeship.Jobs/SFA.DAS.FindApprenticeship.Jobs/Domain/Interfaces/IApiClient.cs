using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
public interface IApiClient
{
    Task<TResponse> Get<TResponse>(IGetApiRequest request);
}
