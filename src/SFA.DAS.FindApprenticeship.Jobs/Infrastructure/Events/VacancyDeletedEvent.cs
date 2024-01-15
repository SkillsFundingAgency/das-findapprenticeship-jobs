using System;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;
public class VacancyDeletedEvent
{
    public int VacancyId { get; set; }
}
