using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Application.Services;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Services.AzureSearchIndexServiceTests;
public class WhenCreatingIndex
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_And_The_Index_Is_Created(
        string indexName,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        AzureSearchIndexService service)
    {
        azureSearchHelper.Setup(x => x.CreateIndex(indexName)).Returns(Task.FromResult(true));

        await service.CreateIndex(indexName);

        azureSearchHelper.Verify(x => x.CreateIndex(indexName), Times.Once);
    }
}