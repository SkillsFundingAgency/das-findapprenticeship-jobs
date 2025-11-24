namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Alerting;

public record AlertMessage(string Origin, string Environment, string Detail, string Timestamp);