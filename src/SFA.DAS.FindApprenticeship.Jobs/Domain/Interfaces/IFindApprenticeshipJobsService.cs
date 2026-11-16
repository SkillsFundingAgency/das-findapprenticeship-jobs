using SFA.DAS.Common.Domain.Models;
using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

public interface IFindApprenticeshipJobsService
{
    Task<GetLiveVacanciesApiResponse> GetLiveVacancies(int pageNumber, int pageSize, DateTime? closingDate = null);
    Task<GetLiveVacancyApiResponse> GetLiveVacancy(VacancyReference vacancyReference);
    Task<GetNhsLiveVacanciesApiResponse?> GetNhsLiveVacancies();
    Task<GetCivilServiceLiveVacanciesApiResponse?> GetCivilServiceLiveVacancies();
    Task SendApplicationClosingSoonReminder(VacancyReference vacancyReference, int daysUntilExpiry);
    Task CloseVacancyEarly(VacancyReference vacancyReference);
    Task<GetCandidateSavedSearchesApiResponse> GetSavedSearches(int pageNumber, int pageSize, string lastRunDateTime);
    Task DeleteSavedSearchesAsync(IEnumerable<Guid> ids);
    Task SendSavedSearchNotification(SavedSearchCandidateVacancies savedSearchCandidateVacancies);
    Task<GetInactiveCandidatesApiResponse> GetDormantCandidates(string cutOffDateTime, int pageNumber, int pageSize);
    Task UpdateCandidateStatus(string govIdentifier, string email, CandidateStatus status);
    Task<SavedSearchCandidateVacancies?> GetSavedSearchResultsForCandidate(SavedSearchResult request);
}