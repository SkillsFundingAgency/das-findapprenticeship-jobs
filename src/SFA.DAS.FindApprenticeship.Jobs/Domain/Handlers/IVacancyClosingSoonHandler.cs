using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;

public interface IVacancyClosingSoonHandler
{
    Task<IList<long>> Handle(int daysToExpire);   
}