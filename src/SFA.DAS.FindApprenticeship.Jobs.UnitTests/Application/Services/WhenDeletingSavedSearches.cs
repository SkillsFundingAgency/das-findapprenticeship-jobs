using SFA.DAS.FindApprenticeship.Jobs.Application.Services;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Services;

public class WhenDeletingSavedSearches
{
    [Test, MoqAutoData]
    public async Task Then_The_Saved_Search_Ids_Are_Passed_To_The_Delete_Api_Call(
        List<Guid> savedSearchIds,
        [Frozen] Mock<IOuterApiClient> apiClient,
        [Greedy] FindApprenticeshipJobsService sut)
    {
        // arrange
        PostDeleteSavedSearchesRequest? capturedRequest = null;
        apiClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(It.IsAny<PostDeleteSavedSearchesRequest>()))
            .Callback<IPostApiRequestWithData>(x => capturedRequest = x as PostDeleteSavedSearchesRequest)
            .ReturnsAsync(new ApiResponse<NullResponse>(null!, HttpStatusCode.NoContent, null!));

        // act
        await sut.DeleteSavedSearchesAsync(savedSearchIds);

        // assert
        capturedRequest.Should().NotBeNull();
        capturedRequest!.Ids.Should().BeEquivalentTo(savedSearchIds);
    }
        
}