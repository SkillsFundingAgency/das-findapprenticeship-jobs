using SFA.DAS.Common.Domain.Models;

namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;

public interface ISendApplicationReminderHandler
{
    Task Handle(VacancyReference vacancyRef, int daysUntilExpiry);
}