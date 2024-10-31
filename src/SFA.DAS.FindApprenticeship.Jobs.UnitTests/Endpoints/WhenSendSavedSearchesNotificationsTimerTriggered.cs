using AutoFixture.NUnit3;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Endpoints;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Endpoints
{
    [TestFixture]
    public class WhenGetAllSavedSearchesNotificationsTimerTriggered
    {
        [Test, MoqAutoData]
        public async Task Then_The_Index_Is_Handled(
            ILogger logger,
            [Frozen] Mock<ICollector<SavedSearchQueueItem>> collector,
            [Frozen] Mock<IGetAllSavedSearchesNotificationHandler> handler,
            SavedSearchesNotificationsTimerTrigger sut)
        {
            await sut.Run(It.IsAny<TimerInfo>(), logger, collector.Object);

            handler.Verify(x => x.Handle(), Times.Once());
        }
    }
}
