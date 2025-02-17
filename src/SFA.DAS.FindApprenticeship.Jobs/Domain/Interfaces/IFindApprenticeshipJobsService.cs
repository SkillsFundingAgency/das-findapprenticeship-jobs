using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
public interface IFindApprenticeshipJobsService
{
    Task<GetLiveVacanciesApiResponse> GetLiveVacancies(int pageNumber, int pageSize, DateTime? closingDate = null);
    Task<GetLiveVacancyApiResponse> GetLiveVacancy(string vacancyReference);
    Task<GetNhsLiveVacanciesApiResponse> GetNhsLiveVacancies();
    Task SendApplicationClosingSoonReminder(long vacancyReference, int daysUntilExpiry);
    Task CloseVacancyEarly(long vacancyRef);
    Task<GetCandidateSavedSearchesApiResponse> GetSavedSearches(int pageNumber, int pageSize, string lastRunDateTime, int maxApprenticeshipSearchResultCount = 5, string sortOrder = "AgeDesc");
    Task SendSavedSearchNotification(SavedSearchCandidateVacancies savedSearchCandidateVacancies);
    Task<GetInactiveCandidatesApiResponse> GetDormantCandidates(string cutOffDateTime, int pageNumber, int pageSize);
    Task UpdateCandidateStatus(string govIdentifier, string email, CandidateStatus status);
    Task<SavedSearchCandidateVacancies?> GetSavedSearchResultsForCandidate(SavedSearchResult request);
}