using System;
using System.Threading.Tasks;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Services;
public class RecruitService : IRecruitService
{
    private readonly IRecruitApiClient _apiClient;

    public RecruitService(IRecruitApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetLiveVacanciesApiResponse> GetLiveVacancies(int pageNumber, int pageSize)
    {
        var liveVacancies = await _apiClient.Get<GetLiveVacanciesApiResponse>(new GetLiveVacanciesApiRequest(pageNumber, pageSize));
        return liveVacancies.Body;
    }

    public async Task<GetLiveVacancyApiResponse> GetLiveVacancy(Guid vacancyId)
    {
        var liveVacancy = await _apiClient.Get<GetLiveVacancyApiResponse>(new GetLiveVacancyApiRequest(vacancyId));
        return liveVacancy.Body;
    }
}
