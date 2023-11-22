using System.Threading.Tasks;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;

namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
public interface IVacancyUpdatedHandler
{
    Task Handle(VacancyUpdatedEvent vacancyUpdatedEvent);
}
