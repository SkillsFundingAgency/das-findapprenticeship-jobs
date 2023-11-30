using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers
{
    public interface IIndexCleanupJobHandler
    {
        Task Handle(ILogger log);
    }
}
