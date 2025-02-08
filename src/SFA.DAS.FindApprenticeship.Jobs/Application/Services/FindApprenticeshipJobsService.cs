using SFA.DAS.FindApprenticeship.Jobs.Domain;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Candidate;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Services;
public class FindApprenticeshipJobsService(IOuterApiClient apiClient) : IFindApprenticeshipJobsService
{
    public async Task<GetLiveVacanciesApiResponse> GetLiveVacancies(int pageNumber, int pageSize, DateTime? closingDate = null)
    {
        var liveVacancies = await apiClient.Get<GetLiveVacanciesApiResponse>(new GetLiveVacanciesApiRequest(pageNumber, pageSize, closingDate));
        return liveVacancies.Body;
    }


    public async Task<GetLiveVacancyApiResponse> GetLiveVacancy(string vacancyReference)
    {
        var liveVacancy = await apiClient.Get<GetLiveVacancyApiResponse>(new GetLiveVacancyApiRequest(vacancyReference));
        return liveVacancy.Body;
    }

    public async Task<GetNhsLiveVacanciesApiResponse> GetNhsLiveVacancies()
    {
        var liveVacancies = await apiClient.Get<GetNhsLiveVacanciesApiResponse>(new GetNhsLiveVacanciesApiRequest());
        return liveVacancies.Body;
    }

    public async Task SendApplicationClosingSoonReminder(long vacancyRef, int daysUntilExpiry)
    {
        await apiClient.Post<NullResponse>(new PostSendApplicationClosingSoonRequest(vacancyRef, daysUntilExpiry));
    }

    public async Task CloseVacancyEarly(long vacancyRef)
    {
        await apiClient.Post<NullResponse>(new PostVacancyClosedEarlyRequest(vacancyRef));
    }

    public async Task<GetCandidateSavedSearchesApiResponse> GetSavedSearches(int pageNumber,
        int pageSize,
        string lastRunDateTime,
        int maxApprenticeshipSearchResultCount = 5,
        string sortOrder = "AgeDesc")
    {
        var savedSearches = await apiClient.Get<GetCandidateSavedSearchesApiResponse>(new GetSavedSearchesApiRequest(
            pageNumber,
            pageSize,
            lastRunDateTime,
            maxApprenticeshipSearchResultCount));
        return savedSearches.Body;
    }

    public async Task SendSavedSearchNotification(SavedSearchCandidateVacancies savedSearchCandidateVacancies)
    {
        await apiClient.PostWithResponseCode<NullResponse>(new PostSendSavedSearchNotificationApiRequest(savedSearchCandidateVacancies));
    }

    public async Task<SavedSearchCandidateVacancies?> GetSavedSearchResultsForCandidate(SavedSearchResult request)
    {
        var actual = await apiClient.PostWithResponseCode<SavedSearchCandidateVacancies>(new PostGetSavedSearchResultsForCandidateRequest(request));
        
        if(actual.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        return actual.Body;
    }

    public async Task<GetInactiveCandidatesApiResponse> GetDormantCandidates(string cutOffDateTime, int pageNumber, int pageSize)
    {
        var candidates = await apiClient.Get<GetInactiveCandidatesApiResponse>(new GetInactiveCandidatesApiRequest(cutOffDateTime, pageNumber, pageSize));
        return candidates.Body;
    }

    public async Task UpdateCandidateStatus(string govIdentifier, string email, CandidateStatus status)
    {
        await apiClient.PostWithResponseCode<NullResponse>(new PostUpdateCandidateStatusApiRequest(govIdentifier, new CandidateUpdateStatusRequest()
        {
            Email = email,
            Status = status
        }));
    }
}
