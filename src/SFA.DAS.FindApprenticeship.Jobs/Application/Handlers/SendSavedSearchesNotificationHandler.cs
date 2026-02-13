using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;

public class SendSavedSearchesNotificationHandler(
    IFindApprenticeshipJobsService findApprenticeshipJobsService)
    : ISendSavedSearchesNotificationHandler
{
    public async Task Handle(SavedSearchCandidateVacancies savedSearchCandidateVacancies)
    {
        await findApprenticeshipJobsService.SendSavedSearchNotification(savedSearchCandidateVacancies);
    }
}