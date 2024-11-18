using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;

namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers
{
    public interface ISendSavedSearchesNotificationHandler
    {
        Task BatchHandle(List<SavedSearch> savedSearches);
        
        Task Handle(SavedSearch savedSearch);
    }
}