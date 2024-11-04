using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;

namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers
{
    public interface ISendSavedSearchesNotificationHandler
    {
        Task Handle(List<SavedSearch> savedSearches);
    }
}