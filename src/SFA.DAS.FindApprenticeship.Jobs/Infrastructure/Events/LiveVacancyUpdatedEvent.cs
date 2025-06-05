using SFA.DAS.Common.Domain.Models;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;

// ReSharper disable once CheckNamespace -- THIS MUST STAY LIKE THIS TO MATCH THE EVENT FROM RECRUIT
namespace Esfa.Recruit.Vacancies.Client.Domain.Events;
public class LiveVacancyUpdatedEvent
{
    public Guid VacancyId { get; set; }
    public VacancyReference VacancyReference { get; set; }
    public LiveUpdateKind UpdateKind { get; set; }
}
