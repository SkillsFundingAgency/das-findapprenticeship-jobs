using System.Threading.Tasks;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers
{
    public class IndexCleanupJobHandler : IIndexCleanupJobHandler
    {
        private readonly IAzureSearchHelper _azureSearchHelperService;
        private readonly IDateTimeService _dateTimeService;

        public IndexCleanupJobHandler(IAzureSearchHelper azureSearchHelperService, IDateTimeService dateTimeService)
        {
            _azureSearchHelperService = azureSearchHelperService;
            _dateTimeService = dateTimeService;
        }

        public async Task Handle()
        {
            



        }
    }
}
