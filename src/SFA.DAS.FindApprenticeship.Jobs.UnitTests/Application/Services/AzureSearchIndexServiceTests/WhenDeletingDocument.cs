using Azure;
using SFA.DAS.FindApprenticeship.Jobs.Application.Services;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Services.AzureSearchIndexServiceTests;
public class WhenDeletingDocument
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_And_The_Document_Is_Deleted(
        string indexName,
        List<string> ids,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        AzureSearchIndexService service)
    {
        azureSearchHelper.Setup(x => x.DeleteDocuments(indexName, ids)).Returns(Task.FromResult(true));

        await service.DeleteDocuments(indexName, ids);

        azureSearchHelper.Verify(x => x.DeleteDocuments(It.IsAny<string>(), ids), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Api_Returns_An_Error(
        string indexName,
        List<string> ids,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        AzureSearchIndexService service)
    {
        azureSearchHelper.Setup(x => x.DeleteDocuments(indexName, ids)).ThrowsAsync(new RequestFailedException("test exception"));

        Func<Task> actual = () => service.DeleteDocuments(indexName, ids);

        await actual.Should().ThrowAsync<RequestFailedException>();
    }
}
