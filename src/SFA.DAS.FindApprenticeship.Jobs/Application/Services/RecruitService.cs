using System.Threading.Tasks;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Services;
public class RecruitService : IRecruitService
{
    private readonly IOuterApiClient _apiClient;

    public RecruitService(IOuterApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetLiveVacanciesApiResponse> GetLiveVacancies(int pageNumber, int pageSize)
    {
        var liveVacancies = await _apiClient.Get<GetLiveVacanciesApiResponse>(new GetLiveVacanciesApiRequest(pageNumber, pageSize));
        return liveVacancies.Body;
    }

    public async Task<GetLiveVacancyApiResponse> GetLiveVacancy(string vacancyReference)
    {
        var liveVacancy = await _apiClient.Get<GetLiveVacancyApiResponse>(new GetLiveVacancyApiRequest(vacancyReference));
        return liveVacancy.Body;
    }

    public async Task<GetNhsLiveVacanciesApiResponse> GetNhsLiveVacancies()
    {
        var liveVacancies = await _apiClient.Get<GetNhsLiveVacanciesApiResponse>(new GetNhsLiveVacanciesApiRequest());
        return liveVacancies.Body;
    }
}
