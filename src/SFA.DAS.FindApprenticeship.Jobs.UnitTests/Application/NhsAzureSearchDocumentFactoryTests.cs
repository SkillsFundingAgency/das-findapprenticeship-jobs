using SFA.DAS.Encoding;
using SFA.DAS.FindApprenticeship.Jobs.Application;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application;

public class NhsAzureSearchDocumentFactoryTests
{
    [Test, MoqAutoData]
    public void Create_Decodes_The_Account_And_Legal_Entity_Hashes(
        GetNhsLiveVacanciesApiResponse.NhsLiveVacancy nhsVacancy,
        [Frozen] Mock<IEncodingService> encodingService,
        NhsAzureSearchDocumentFactory sut)
    {
        // arrange
        encodingService.Setup(x => x.Decode(nhsVacancy.AccountPublicHashedId, EncodingType.PublicAccountId)).Returns(888);
        encodingService.Setup(x => x.Decode(nhsVacancy.AccountLegalEntityPublicHashedId, EncodingType.PublicAccountLegalEntityId)).Returns(999);

        // act
        var searchDocument = sut.Create(nhsVacancy);

        // assert
        searchDocument.AccountId.Should().Be(888);
        searchDocument.AccountLegalEntityId.Should().Be(999);
    }
}