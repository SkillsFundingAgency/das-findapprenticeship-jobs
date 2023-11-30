using System;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;
public class VacancyUpdatedEvent
{
    public Guid VacancyId { get; set; }
    public long VacancyReference { get; set; }
    public LiveUpdateKind UpdateKind { get; set; }
}
