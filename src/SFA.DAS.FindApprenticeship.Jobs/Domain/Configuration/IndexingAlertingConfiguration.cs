namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;

public class IndexingAlertingConfiguration
{
    public int DocumentDecreasePercentageThreshold { get; init; } = 50;
    public bool Enabled { get; init; }
    public string TeamsAlertWebhookUrl { get; init; } = string.Empty;
}