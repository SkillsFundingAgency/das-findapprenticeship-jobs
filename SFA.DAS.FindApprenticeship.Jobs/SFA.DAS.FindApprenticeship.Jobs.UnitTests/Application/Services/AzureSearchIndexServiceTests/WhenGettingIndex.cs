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

        var result = await service.GetIndex(indexName);

        using (new AssertionScope())
        {
            azureSearchHelper.Verify(x => x.GetIndex(indexName), Times.Once);
            result.Value.GetType().Should().Be(typeof(SearchIndex));
        }
    }
}