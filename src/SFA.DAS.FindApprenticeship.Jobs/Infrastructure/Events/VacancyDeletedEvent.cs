using System;

namespace Esfa.Recruit.Vacancies.Client.Domain.Events;
public class VacancyDeletedEvent
{
    public Guid VacancyId { get; set; }
}
