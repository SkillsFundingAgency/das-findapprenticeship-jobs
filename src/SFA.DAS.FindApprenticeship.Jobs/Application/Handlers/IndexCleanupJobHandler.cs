using System.Globalization;
using Azure.Search.Documents.Indexes.Models;
using SFA.DAS.FindApprenticeship.Jobs.Domain;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;

public class IndexCleanupJobHandler(
    IAzureSearchHelper azureSearchHelperService,
    ILogger<IndexCleanupJobHandler> log)
    : IIndexCleanupJobHandler
{
    private const int MaxIndexCount = 6;
    private const int CriticalIndexCount = 12;

    public async Task Handle()
    {
        var aliasTask = azureSearchHelperService.GetAlias(Constants.AliasName);
        var indexesTask = azureSearchHelperService.GetIndexes();

        await Task.WhenAll(aliasTask, indexesTask);

        var indexes = indexesTask.Result;
        var alias = aliasTask.Result;

        var aliasTarget = alias == null ? string.Empty : alias.Indexes.FirstOrDefault();

        int indexDeletionCount = 0;
        var recruitIndexes = indexes.Where(x => x.Name.StartsWith(Constants.IndexPrefix) && x.Name != aliasTarget).ToList();
        if (recruitIndexes.Count > MaxIndexCount)
        {
            Dictionary<DateTime, SearchIndex> deletionCandidates = new();
            recruitIndexes.ForEach(x =>
            {
                if (TryGetDate(x.Name, out var parsedDate))
                {
                    deletionCandidates.Add(parsedDate, x);
                }
                else
                {
                    log.LogInformation("Index name {indexName} does not contain a valid timestamp.", x.Name);
                }
            });
                
            var indexesToDelete = deletionCandidates.OrderByDescending(x => x.Key).Skip(MaxIndexCount);
            foreach (var kvp in indexesToDelete)
            {
                log.LogInformation("Deleting index {indexName}", kvp.Value.Name);
                await azureSearchHelperService.DeleteIndex(kvp.Value.Name);
                indexDeletionCount++;
            }
        }

        if (indexes.Count >= CriticalIndexCount && indexDeletionCount == 0)
        {
            log.LogCritical("Index threshold reached but no indexes were deleted, further indexing may fail.");
        }
    }

    private static bool TryGetDate(string name, out DateTime date)
    {
        return DateTime.TryParseExact(
            name[Constants.IndexPrefix.Length..],
            Constants.IndexDateSuffixFormat,
            CultureInfo.InvariantCulture, DateTimeStyles.None,
            out date
        );
    }
}