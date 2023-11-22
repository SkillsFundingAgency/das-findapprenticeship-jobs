using System.Threading.Tasks;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
public class VacancyUpdatedHandler : IVacancyUpdatedHandler
{
    public VacancyUpdatedHandler()
    {
        
    }

    public Task Handle(VacancyUpdatedEvent vacancyUpdatedEvent)
    {
        throw new System.NotImplementedException();
    }
}
