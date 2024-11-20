using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;
using SFA.DAS.FindApprenticeship.Jobs.Endpoints;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Endpoints;

[TestFixture]
public class WhenSavedSearchesNotificationsQueueTriggered
{
    [Test, MoqAutoData]
    public async Task Then_The_Queue_Item_Is_Processed_And_SendReminderHandler_Called(
        SavedSearchQueueItem mockSavedSearchQueueItem,
        [Frozen] Mock<ISendSavedSearchesNotificationHandler> handler,
        SendSavedSearchesNotificationsQueueTrigger trigger)
    {
        await trigger.Run(mockSavedSearchQueueItem);

        handler.Verify(x => x.Handle(It.IsAny<SavedSearch>()), Times.Once());
    }
}