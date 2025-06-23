using Microsoft.Extensions.Options;
using SFA.DAS.FindApprenticeship.Jobs.Application.Services;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Slack;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Indexing;

public interface IIndexingAlertsManager
{
    Task VerifySnapshotsAsync(IndexStatistics? oldStats, IndexStatistics? newStats, CancellationToken cancellationToken = default);
    Task SendNhsApiAlertAsync(CancellationToken cancellationToken = default);
    Task SendNhsImportAlertAsync(CancellationToken cancellationToken = default);
}

public class IndexingAlertsManager(
    IOptions<IndexingAlertConfiguration> config,
    FunctionEnvironment environment,
    ISlackClient slackClient,
    ILogger<IndexingAlertsManager> logger): IIndexingAlertsManager
{
    private readonly List<Block> _messageTemplate =
    [
        new HeaderBlock(new PlainText(":exclamation: FAA Indexing Issue", true)),
        new SectionBlock(new MarkdownText($"Environment: *{environment.EnvironmentName}*")),
        new DividerBlock()
    ];
    private Block IssueBlock(string text) => new SectionBlock(new MarkdownText($"Issue: *{text}*"));
    private List<Block> IndexEmptyMessage => [.._messageTemplate, IssueBlock("the index contains no documents")];
    private List<Block> NoNhsVacanciesImported => [.._messageTemplate, IssueBlock("no NHS vacancies were imported")];
    private List<Block> NoNhsVacanciesReturned => [.._messageTemplate, IssueBlock("the external NHS API returned no vacancies")];
    private List<Block> IndexThresholdBreachedMessage(int value) => [.._messageTemplate, IssueBlock($"a {value}% decrease in documents has been detected")];

    public async Task VerifySnapshotsAsync(IndexStatistics? oldStats, IndexStatistics? newStats, CancellationToken cancellationToken = default)
    {
        try
        {
            if (oldStats is null || newStats is null)
            {
                return;
            }
            await CompareSnapshotsAsync(oldStats.Value, newStats.Value, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occured whilst trying to verify the index statistics");
        }
    }

    public async Task SendNhsApiAlertAsync(CancellationToken cancellationToken = default)
    {
        await SendAlertAsync(NoNhsVacanciesReturned, cancellationToken);
    }

    public async Task SendNhsImportAlertAsync(CancellationToken cancellationToken = default)
    {
        await SendAlertAsync(NoNhsVacanciesImported, cancellationToken);
    }
    
    private async Task SendAlertAsync(List<Block> blocksMessage, CancellationToken cancellationToken = default)
    {
        var channels = config.Value.Channels ?? [];
        try
        {
            foreach (var channel in channels)
            {
                var postMessageRequest = new SlackMessage(channel, blocksMessage);
                var result = await slackClient.PostMessageAsync(postMessageRequest, cancellationToken);
                if (!result.Ok)
                {
                    logger.LogWarning("Slack PostMessage failed. Response error: {ResponseError}", result.Error);
                }
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Exception occured whilst trying to send an alert.");
        }
    }

    private async Task CompareSnapshotsAsync(IndexStatistics before, IndexStatistics after, CancellationToken cancellationToken = default)
    {
        var countAfter = after.DocumentCount;
        if (countAfter is 0)
        {
            await SendAlertAsync(IndexEmptyMessage, cancellationToken);
            return;
        }
        
        var countBefore = before.DocumentCount;
        var diff = countAfter - countBefore;
        if (diff is not 0)
        {
            var change = countBefore is 0 ? 0 : (double)diff / countBefore * 100;
            if (change <= -config.Value.DocumentDecreasePercentageThreshold)
            {
                await SendAlertAsync(IndexThresholdBreachedMessage((int)Math.Round(change)), cancellationToken);
            }    
        }
    }
}