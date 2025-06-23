namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;

public record IndexingAlertConfiguration(int DocumentDecreasePercentageThreshold = 50, List<string>? Channels = null);