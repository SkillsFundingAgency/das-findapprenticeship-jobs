namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;

public record IndexingAlertingConfiguration(string TeamsAlertWebhookUrl, int DocumentDecreasePercentageThreshold = 50);