using Esfa.Recruit.Vacancies.Client.Domain.Events;

namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
public interface IVacancyUpdatedHandler
{
    Task Handle(LiveVacancyUpdatedEvent vacancyUpdatedEvent);
}
