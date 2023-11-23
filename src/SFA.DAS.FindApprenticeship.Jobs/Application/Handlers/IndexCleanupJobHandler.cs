using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Azure.Search.Documents.Indexes.Models;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Application.Extensions;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers
{
    public class IndexCleanupJobHandler : IIndexCleanupJobHandler
    {
        private readonly IAzureSearchHelper _azureSearchHelperService;
        private readonly IDateTimeService _dateTimeService;
        private readonly ILogger<IndexCleanupJobHandler> _logger;
        private readonly TimeSpan _indexDeletionAgeThreshold = new(0, 6, 0, 0);

        public IndexCleanupJobHandler(IAzureSearchHelper azureSearchHelperService, IDateTimeService dateTimeService, ILogger<IndexCleanupJobHandler> logger)
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

            var indexes = indexesTask.Result;
            var alias = aliasTask.Result;

            var aliasTarget = alias == null ? string.Empty : alias.Indexes.FirstOrDefault();

            foreach (var index in indexes.Where(IsApprenticeshipsIndex))
            {
                if (index.Name == aliasTarget)
                {
                    _logger.LogInformation($"Skipping index {index.Name} as currently aliased");
                    continue;
                }

                if (IsDeletionCandidate(index))
                {
                    _logger.LogInformation($"Deleting index {index.Name}");
                    await _azureSearchHelperService.DeleteIndex(index.Name);
                }
            }
        }

        private bool IsApprenticeshipsIndex(SearchIndex index)
        {
            return index.Name.StartsWith($"{Domain.Constants.IndexPrefix}");
        }

        private bool IsDeletionCandidate(SearchIndex index)
        {
            var dateSuffix = index.Name.Substring(Domain.Constants.IndexPrefix.Length);

            if (!DateTime.TryParseExact(dateSuffix, Domain.Constants.IndexDateSuffixFormat,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                _logger.LogWarning($"Unable to parse date from index {index.Name}");
                return false;
            }

            var age = _dateTimeService.GetCurrentDateTime().Subtract(parsedDate).RemoveSeconds();

            return age > _indexDeletionAgeThreshold;
        }
    }
}
