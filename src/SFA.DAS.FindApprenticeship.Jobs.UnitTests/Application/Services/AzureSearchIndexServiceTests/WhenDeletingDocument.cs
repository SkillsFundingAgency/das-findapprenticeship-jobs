using AutoFixture.NUnit3;
using Azure;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Application.Services;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Services.AzureSearchIndexServiceTests;
public class WhenDeletingDocument
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_And_The_Document_Is_Deleted(
        string indexName,
        Guid vacancyId,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        AzureSearchIndexService service)
    {
        azureSearchHelper.Setup(x => x.DeleteDocument(indexName, $"VAC{vacancyId}")).Returns(Task.FromResult(true));

        await service.DeleteDocument(indexName, $"VAC{vacancyId}");

        azureSearchHelper.Verify(x => x.DeleteDocument(It.IsAny<string>(), $"VAC{vacancyId}"), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Api_Returns_An_Error(
        string indexName,
        Guid vacancyId,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        AzureSearchIndexService service)
    {
        azureSearchHelper.Setup(x => x.DeleteDocument(indexName, $"VAC{vacancyId}")).ThrowsAsync(new RequestFailedException("test exception"));

        Func<Task> actual = () => service.DeleteDocument(indexName, $"VAC{vacancyId}");

        await actual.Should().ThrowAsync<RequestFailedException>();
    }
}
