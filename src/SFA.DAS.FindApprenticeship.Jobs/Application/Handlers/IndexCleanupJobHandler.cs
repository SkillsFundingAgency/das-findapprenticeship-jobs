using System.Globalization;
using Azure.Search.Documents.Indexes.Models;
using SFA.DAS.FindApprenticeship.Jobs.Application.Extensions;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers
{
    public class IndexCleanupJobHandler : IIndexCleanupJobHandler
    {
        private readonly IAzureSearchHelper _azureSearchHelperService;
        private readonly IDateTimeService _dateTimeService;
        private readonly ILogger<IndexCleanupJobHandler> _log;
        private readonly TimeSpan _indexDeletionAgeThreshold = new(0, 12, 0, 0);

        public IndexCleanupJobHandler(IAzureSearchHelper azureSearchHelperService, IDateTimeService dateTimeService, ILogger<IndexCleanupJobHandler> log)
        {
            _azureSearchHelperService = azureSearchHelperService;
            _dateTimeService = dateTimeService;
            _log = log;
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
                    _log.LogInformation($"Skipping index {index.Name} as currently aliased");
                    continue;
                }

                if (IsDeletionCandidate(index, _log))
                {
                    _log.LogInformation($"Deleting index {index.Name}");
                    await _azureSearchHelperService.DeleteIndex(index.Name);
                }
            }
        }

        private bool IsApprenticeshipsIndex(SearchIndex index)
        {
            return index.Name.StartsWith($"{Domain.Constants.IndexPrefix}");
        }

        private bool IsDeletionCandidate(SearchIndex index, ILogger log)
        {
            var dateSuffix = index.Name.Substring(Domain.Constants.IndexPrefix.Length);

            if (!DateTime.TryParseExact(dateSuffix, Domain.Constants.IndexDateSuffixFormat,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                log.LogWarning($"Unable to parse date from index {index.Name}");
                return false;
            }

            var age = _dateTimeService.GetCurrentDateTime().Subtract(parsedDate).RemoveSeconds();

            return age > _indexDeletionAgeThreshold;
        }
    }
}
