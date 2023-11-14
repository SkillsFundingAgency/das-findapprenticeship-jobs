using AutoFixture.NUnit3;
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
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        AzureSearchIndexService service)
    {
        azureSearchHelper.Setup(x => x.UploadDocuments(documents)).Returns(Task.FromResult(true));

        await service.UploadDocuments(documents);

        azureSearchHelper.Verify(x => x.UploadDocuments(documents), Times.Once);
    }
}
