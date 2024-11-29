using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Services;
public class FindApprenticeshipJobsService : IFindApprenticeshipJobsService
{
    private readonly IOuterApiClient _apiClient;

    public FindApprenticeshipJobsService(IOuterApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetLiveVacanciesApiResponse> GetLiveVacancies(int pageNumber, int pageSize, DateTime? closingDate = null)
    {
        var liveVacancies = await _apiClient.Get<GetLiveVacanciesApiResponse>(new GetLiveVacanciesApiRequest(pageNumber, pageSize, closingDate));
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

    public async Task SendApplicationClosingSoonReminder(long vacancyRef, int daysUntilExpiry)
    {
        await _apiClient.Post<NullResponse>(new PostSendApplicationClosingSoonRequest(vacancyRef, daysUntilExpiry));
    }

    public async Task CloseVacancyEarly(long vacancyRef)
    {
        await _apiClient.Post<NullResponse>(new PostVacancyClosedEarlyRequest(vacancyRef));
    }

    public async Task<GetSavedSearchesApiResponse> GetSavedSearches(int pageNumber,
        int pageSize,
        string lastRunDateTime,
        int maxApprenticeshipSearchResultCount = 5,
        string sortOrder = "AgeDesc")
    {
        var savedSearches = await _apiClient.Get<GetSavedSearchesApiResponse>(new GetSavedSearchesApiRequest(
            pageNumber,
            pageSize,
            lastRunDateTime,
            maxApprenticeshipSearchResultCount));
        return savedSearches.Body;
    }

    public async Task SendSavedSearchNotification(SavedSearch savedSearch)
    {
        await _apiClient.PostWithResponseCode<NullResponse>(new PostSendSavedSearchNotificationApiRequest(savedSearch));
    }
}
