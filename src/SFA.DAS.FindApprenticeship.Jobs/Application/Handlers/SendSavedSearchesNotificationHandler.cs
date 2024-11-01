using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers
{
    public class SendSavedSearchesNotificationHandler : ISendSavedSearchesNotificationHandler
    {
        private readonly IFindApprenticeshipJobsService _findApprenticeshipJobsService;

        public SendSavedSearchesNotificationHandler(IFindApprenticeshipJobsService findApprenticeshipJobsService)
        {
            _findApprenticeshipJobsService = findApprenticeshipJobsService;
        }

        public async Task Handle(List<SavedSearch> savedSearches)
        {
            foreach (var savedSearch in savedSearches)
            {
                await _findApprenticeshipJobsService.SendSavedSearchNotification(savedSearch);
            }
        }
    }
}
