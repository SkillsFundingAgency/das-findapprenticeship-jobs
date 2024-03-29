﻿using AutoFixture.NUnit3;
using Azure;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Application.Services;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Services.AzureSearchIndexServiceTests;
public class WhenUploadingDocuments
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_And_The_Documents_Are_Uploaded(
        IEnumerable<ApprenticeAzureSearchDocument> documents,
        string indexName,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        AzureSearchIndexService service)
    {

        azureSearchHelper.Setup(x => x.UploadDocuments(indexName, documents)).Returns(Task.FromResult(true));

        await service.UploadDocuments(indexName, documents);

        azureSearchHelper.Verify(x => x.UploadDocuments(indexName, documents), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Api_Returns_An_Error(
        IEnumerable<ApprenticeAzureSearchDocument> documents,
        string indexName,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        AzureSearchIndexService service)
    {
        azureSearchHelper.Setup(x => x.UploadDocuments(indexName, documents)).ThrowsAsync(new RequestFailedException("test exception"));

        Func<Task> actual = () => service.UploadDocuments(indexName,documents);

        await actual.Should().ThrowAsync<RequestFailedException>();
    }
}
