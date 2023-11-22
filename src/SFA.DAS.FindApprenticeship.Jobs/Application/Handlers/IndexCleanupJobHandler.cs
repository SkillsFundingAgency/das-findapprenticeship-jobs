using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.FindApprenticeship.Jobs.Application.Extensions;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers
{
    public class IndexCleanupJobHandler : IIndexCleanupJobHandler
    {
        private readonly IAzureSearchHelper _azureSearchHelperService;
        private readonly IDateTimeService _dateTimeService;
        private readonly ILogger _logger;

        public IndexCleanupJobHandler(IAzureSearchHelper azureSearchHelperService, IDateTimeService dateTimeService, ILogger logger)
        {
            _azureSearchHelperService = azureSearchHelperService;
            _dateTimeService = dateTimeService;
            _logger = logger;
        }

        public async Task Handle()
        {
            var aliasTask = _azureSearchHelperService.GetAlias(Domain.Constants.AliasName);
            var indexesTask = _azureSearchHelperService.GetIndexes();

            await Task.WhenAll(aliasTask, indexesTask);

            var alias = aliasTask.Result;
            var indexes = indexesTask.Result;

            foreach (var index in indexes.Where(x => x.Name.StartsWith($"{Domain.Constants.IndexPrefix}")))
            {
                if (alias != null && alias.Indexes.Contains(index.Name))
                {
                    _logger.LogInformation($"Skipping index {index.Name} as currently aliased");
                    continue;
                }

                var dateSuffix = index.Name.Substring(16);

                if (DateTime.TryParse(dateSuffix, out var parsedDate))
                {
                    var age = _dateTimeService.GetCurrentDateTime().Subtract(parsedDate).RemoveSeconds();

                    if (age > new TimeSpan(0, 6, 0, 0))
                    {
                        _logger.LogInformation($"Deleting index {index.Name}, age {age.ToHumanReadableString()}");
                        await _azureSearchHelperService.DeleteIndex(index.Name);
                    }
                }
            }
        }
    }
}
