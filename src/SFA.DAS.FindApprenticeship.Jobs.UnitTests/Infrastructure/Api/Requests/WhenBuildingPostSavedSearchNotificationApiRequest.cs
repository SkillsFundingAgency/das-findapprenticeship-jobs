﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Domain.SavedSearches;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Infrastructure.Api.Requests
{
    [TestFixture]
    public class WhenBuildingPostSavedSearchNotificationApiRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Built(SavedSearch savedSearch)
        {
            var actual = new PostSendSavedSearchNotificationApiRequest(savedSearch);

            actual.PostUrl.Should().Be("/savedSearches/sendNotification");
        }
    }
}