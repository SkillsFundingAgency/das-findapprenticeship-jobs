﻿using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;
using SFA.DAS.FindApprenticeship.Jobs.Endpoints;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Endpoints
{
    [TestFixture]
    public class WhenGetAllSavedSearchesNotificationsTimerTriggered
    {
        [Test, MoqAutoData]
        public async Task Then_The_Index_Is_Handled(
            ILogger logger,
            SavedSearch mockSavedSearch,
            [Frozen] Mock<IGetAllSavedSearchesNotificationHandler> handler,
            SavedSearchesNotificationsTimerTrigger sut)
        {
            handler.Setup(x => x.Handle()).ReturnsAsync([mockSavedSearch]);

            var collector = await sut.Run(It.IsAny<TimerInfo>());

            handler.Verify(x => x.Handle(), Times.Once());

            collector.Should().BeEquivalentTo([mockSavedSearch]);
        }
    }
}
