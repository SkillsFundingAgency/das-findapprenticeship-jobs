namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;

public interface ISendApplicationReminderHandler
{
    Task Handle(long vacancyRef, int daysUntilExpiry);
}