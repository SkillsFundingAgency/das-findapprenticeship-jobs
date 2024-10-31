using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;

namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers
{
    public interface IGetAllSavedSearchesNotificationHandler
    {
        Task<List<SavedSearch>> Handle();
    }
}
