using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;

namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
public interface IVacancyUpdatedHandler
{
    Task Handle(LiveVacancyUpdatedEvent vacancyUpdatedEvent);
}
