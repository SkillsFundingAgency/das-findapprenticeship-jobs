using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Handlers
{
    [TestFixture]
    public class WhenHandlingSendSavedSearchNotification
    {
        [Test, MoqAutoData]
        public async Task Then_The_Notification_Is_Sent(
        List<SavedSearch> mockSavedSearches,
        [Frozen] Mock<IFindApprenticeshipJobsService> findApprenticeshipJobsService,
        SendSavedSearchesNotificationHandler handler)
        {
            await handler.Handle(mockSavedSearches);

            findApprenticeshipJobsService.Verify(
                x => x.SendSavedSearchNotification(It.IsAny<SavedSearch>()), Times.Exactly(mockSavedSearches.Count));
        }
    }
}
