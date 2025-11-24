namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;

public class IndexingAlertingConfiguration
{
    public required string TeamsAlertWebhookUrl { get; init; }
    public int DocumentDecreasePercentageThreshold { get; init; } = 50;
}