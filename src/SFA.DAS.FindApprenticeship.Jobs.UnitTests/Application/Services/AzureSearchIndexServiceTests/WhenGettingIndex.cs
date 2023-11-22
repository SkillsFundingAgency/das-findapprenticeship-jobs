using AutoFixture.NUnit3;
using Azure;
using Azure.Search.Documents.Indexes.Models;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Application.Services;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Services.AzureSearchIndexServiceTests;
public class WhenGettingIndex
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Api_Is_Called_And_The_Index_Is_Fetched(
        CorsOptions corsOptions,
        string defaultScoringProfile,
        SearchResourceEncryptionKey encryptionKey,
        ETag eTag,
        ClassicSimilarity similarity,
        string indexName,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        AzureSearchIndexService service)
    {
        var response = new SearchIndex("index")
        {
            CorsOptions = corsOptions,
            DefaultScoringProfile = defaultScoringProfile,
            EncryptionKey = encryptionKey,
            ETag = eTag,
            Similarity = similarity
        };
        azureSearchHelper.Setup(x => x.GetIndex(indexName)).ReturnsAsync(Response.FromValue(response, null!));

        var actual = await service.GetIndex(indexName);

        using (new AssertionScope())
        {
            azureSearchHelper.Verify(x => x.GetIndex(indexName), Times.Once);
            actual.Value.GetType().Should().Be(typeof(SearchIndex));
        }
    }

    [Test, MoqAutoData]
    public async Task Then_The_Api_Returns_An_Error(
        string indexName,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        AzureSearchIndexService service)
    {
        azureSearchHelper.Setup(x => x.GetIndex(indexName)).ThrowsAsync(new RequestFailedException("test exception"));

        Func<Task> actual = () => service.GetIndex(indexName);

        await actual.Should().ThrowAsync<RequestFailedException>();
    }
}