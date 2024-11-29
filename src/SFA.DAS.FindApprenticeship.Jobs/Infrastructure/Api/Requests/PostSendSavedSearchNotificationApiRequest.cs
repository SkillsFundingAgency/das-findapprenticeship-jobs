using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests
{
    public class PostSendSavedSearchNotificationApiRequest : IPostApiRequestWithData
    {
        public string PostUrl => "savedSearches/sendNotification";
        public object Data { get; set; }

        public PostSendSavedSearchNotificationApiRequest(SavedSearch savedSearch)
        {
            Data = savedSearch;
        }
    }
}