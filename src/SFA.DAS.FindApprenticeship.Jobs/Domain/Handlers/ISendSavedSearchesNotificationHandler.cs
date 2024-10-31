using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers
{
    public interface ISendSavedSearchesNotificationHandler
    {
        Task Handle(List<SavedSearch> savedSearches);
    }
}