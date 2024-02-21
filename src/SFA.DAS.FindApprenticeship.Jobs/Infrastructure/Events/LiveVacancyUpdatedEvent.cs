using System;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;

namespace Esfa.Recruit.Vacancies.Client.Domain.Events;
public class LiveVacancyUpdatedEvent
{
    public Guid VacancyId { get; set; }
    public long VacancyReference { get; set; }
    public LiveUpdateKind UpdateKind { get; set; }
}
