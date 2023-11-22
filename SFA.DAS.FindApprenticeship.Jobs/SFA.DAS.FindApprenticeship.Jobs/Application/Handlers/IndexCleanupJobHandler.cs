using System;
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
            var indexes = await _azureSearchHelperService.GetIndexes();

            foreach (var index in indexes)
            {
                if (index.Name.StartsWith("apprenticeships_"))
                {
                    var dateSuffix = index.Name.Substring(16);

                    if (DateTime.TryParse(dateSuffix, out var parsedDate))
                    {
                        var age = _dateTimeService.GetCurrentDateTime().Subtract(parsedDate);

                        if (age > new TimeSpan(0, 6, 0, 0))
                        {
                            await _azureSearchHelperService.DeleteIndex(index.Name);
                        }
                    }
                }
            }
        }
    }
}
