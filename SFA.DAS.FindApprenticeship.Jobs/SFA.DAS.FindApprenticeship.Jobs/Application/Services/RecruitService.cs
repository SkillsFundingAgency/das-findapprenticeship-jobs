﻿using System.Threading.Tasks;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Services;
public class RecruitService : IRecruitService
{
    private readonly IApiClient _apiClient;

    public RecruitService(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetLiveVacanciesApiResponse> GetLiveVacancies(int pageNumber, int pageSize)
    {
        var liveVacancies = await _apiClient.Get<GetLiveVacanciesApiResponse>(new GetLiveVacanciesRequest(pageNumber, pageSize));
        return liveVacancies;
    }
}
