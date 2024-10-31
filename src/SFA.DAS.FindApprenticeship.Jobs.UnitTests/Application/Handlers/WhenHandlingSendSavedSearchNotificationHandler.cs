using AutoFixture.NUnit3;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses.GetSavedSearchesApiResponse;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Handlers
{
    [TestFixture]
    public class WhenHandlingSendSavedSearchNotificationHandler
    {
        [Test, MoqAutoData]
        public async Task Then_The_SavedSearches_Are_Retrieved_And_Items_Added_To_Queue_Is_Created(
            List<SavedSearchResult> mockSavedSearchResult,
            [Frozen] Mock<IFindApprenticeshipJobsService> findApprenticeShipJobsService,
            [Frozen] Mock<IDateTimeService> dateTimeService,
           DateTime currentDateTime,
           GetAllSavedSearchesNotificationHandler sut)
        {
            const int pageNumber = 1;
            const int pageSize = 1000;
            const int maxApprenticeshipSearchResultCount = 10;
            const string sortOrder = "AgeDesc";

            dateTimeService.Setup(x => x.GetCurrentDateTime()).Returns(currentDateTime);

            var mockGetSavedSearchesApiResponse = new GetSavedSearchesApiResponse
            {
                SavedSearchResults = mockSavedSearchResult,
                PageIndex = 1,
                TotalCount = mockSavedSearchResult.Count,
                PageSize = mockSavedSearchResult.Count,
                TotalPages = 100
            };

            findApprenticeShipJobsService.Setup(x =>
                x.GetSavedSearches(pageNumber, pageSize, currentDateTime.ToString("O"), maxApprenticeshipSearchResultCount, sortOrder))
                .ReturnsAsync(mockGetSavedSearchesApiResponse);

            await sut.Handle();

            using (new AssertionScope())
            {
                findApprenticeShipJobsService.Verify(x => x.GetSavedSearches(pageNumber, pageSize, currentDateTime.ToString("O"), maxApprenticeshipSearchResultCount, sortOrder),
                    Times.AtLeastOnce());
            }
        }
    }
}
