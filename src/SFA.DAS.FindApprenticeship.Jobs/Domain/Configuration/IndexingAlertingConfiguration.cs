namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;

public interface IIndexingAlertingConfiguration
{
    int DocumentDecreasePercentageThreshold { get; }
    string TeamsAlertWebhookUrl { get; }
}

public class IndexingAlertingConfiguration: IIndexingAlertingConfiguration
{
    public int DocumentDecreasePercentageThreshold { get; init; } = 50;
    public bool Enabled { get; init; }
    public string TeamsAlertWebhookUrl { get; init; } = string.Empty;
}