using Esfa.Recruit.Vacancies.Client.Domain.Events;

namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
public interface IVacancyClosedHandler
{
    Task Handle(VacancyClosedEvent vacancyClosedEvent);
}
