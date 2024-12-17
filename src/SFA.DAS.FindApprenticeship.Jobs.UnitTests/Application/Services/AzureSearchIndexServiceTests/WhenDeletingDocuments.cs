using AutoFixture.NUnit3;
using Azure;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Application.Services;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Services.AzureSearchIndexServiceTests;
public class WhenDeletingDocuments
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_And_The_Document_Is_Deleted(
        string indexName,
        List<string> vacancyIds,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        AzureSearchIndexService service)
    {
        azureSearchHelper.Setup(x => x.DeleteDocuments(indexName, vacancyIds)).Returns(Task.FromResult(true));

        await service.DeleteDocuments(indexName, vacancyIds);

        azureSearchHelper.Verify(x => x.DeleteDocuments(It.IsAny<string>(), vacancyIds), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Api_Returns_An_Error(
        string indexName,
        List<string> vacancyIds,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        AzureSearchIndexService service)
    {
        azureSearchHelper.Setup(x => x.DeleteDocuments(indexName, vacancyIds)).ThrowsAsync(new RequestFailedException("test exception"));

        Func<Task> actual = () => service.DeleteDocuments(indexName, vacancyIds);

        await actual.Should().ThrowAsync<RequestFailedException>();
    }
}
