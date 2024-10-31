using System;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;
public class VacancyClosedEvent
{
    public Guid VacancyId { get; set; }
    public long VacancyReference { get; set; }
}
