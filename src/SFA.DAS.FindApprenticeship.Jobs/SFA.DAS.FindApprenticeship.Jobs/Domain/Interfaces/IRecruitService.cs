using System.Threading.Tasks;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
public interface IRecruitService
{
    Task<GetLiveVacanciesApiResponse> GetLiveVacancies(int pageNumber, int pageSize);
}
