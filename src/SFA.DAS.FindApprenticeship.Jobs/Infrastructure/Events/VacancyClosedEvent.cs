using System;

namespace Esfa.Recruit.Vacancies.Client.Domain.Events;
public class VacancyClosedEvent
{
    public Guid VacancyId { get; set; }
}
