using Azure;
using Azure.Search.Documents.Indexes.Models;
using Esfa.Recruit.Vacancies.Client.Domain.Events;
using SFA.DAS.FindApprenticeship.Jobs.Application;
using SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Handlers;

[TestFixture]
public class WhenHandlingVacancyApprovedEvent
{
    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_Is_Uploaded_To_The_Index(
        ILogger log,
        VacancyApprovedEvent vacancyApprovedEvent,
        string indexName,
        int programmeId,
        Response<ApprenticeAzureSearchDocument> document,
        Response<GetLiveVacancyApiResponse> liveVacancy,
        [Frozen] Mock<IFindApprenticeshipJobsService> findApprenticeshipJobsService,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        [Frozen] Mock<IApprenticeAzureSearchDocumentFactory> documentFactory,
        VacancyApprovedHandler sut)
    {
        // arrange
        liveVacancy.Value.StandardLarsCode = programmeId;

        findApprenticeshipJobsService.Setup(x => x.GetLiveVacancy(vacancyApprovedEvent.VacancyReference)).ReturnsAsync(liveVacancy);
        azureSearchHelper.Setup(x => x.GetAlias(Constants.AliasName))
            .ReturnsAsync(() => new SearchAlias(Constants.AliasName, new[] { indexName }));
        azureSearchHelper.Setup(x => x.GetAlias(Constants.AliasName)).ReturnsAsync(() => new SearchAlias(Constants.AliasName, [indexName]));
        documentFactory.Setup(x => x.Create(liveVacancy)).Returns([document.Value]);

        // act
        await sut.Handle(vacancyApprovedEvent);

        // assert
        azureSearchHelper.Verify(
            x => x.UploadDocuments(indexName, 
                It.Is<IEnumerable<ApprenticeAzureSearchDocument>>(d => d.Single() == document.Value)),
            Times.Once()
        );
    }

    [Test, MoqAutoData]
    public async Task Then_The_Event_Is_Ignored_If_No_Index_Is_Currently_Aliased(
        ILogger log,
        VacancyApprovedEvent vacancyApprovedEvent,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        VacancyApprovedHandler sut)
    {
        azureSearchHelper.Setup(x => x.GetAlias(Constants.AliasName))
            .ReturnsAsync(() => null);

        await sut.Handle(vacancyApprovedEvent);

        azureSearchHelper.Verify(x => x.UploadDocuments(It.IsAny<string>(), It.IsAny<IEnumerable<ApprenticeAzureSearchDocument>>()),
            Times.Never());
    }

    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_With_OtherAddresses_Is_Uploaded_To_The_Index(
        List<Address> otherAddresses,
        ILogger log,
        VacancyApprovedEvent vacancyApprovedEvent,
        string indexName,
        int programmeId,
        GetLiveVacancyApiResponse liveVacancy,
        List<ApprenticeAzureSearchDocument> azureSearchDocuments,
        [Frozen] Mock<IFindApprenticeshipJobsService> findApprenticeshipJobsService,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        [Frozen] Mock<IApprenticeAzureSearchDocumentFactory> azureDocumentFactory,
        VacancyApprovedHandler sut)
    {
        liveVacancy.EmploymentLocations = otherAddresses;
        liveVacancy.StandardLarsCode = programmeId;

        findApprenticeshipJobsService.Setup(x => x.GetLiveVacancy(vacancyApprovedEvent.VacancyReference))
            .ReturnsAsync(liveVacancy);
        azureSearchHelper.Setup(x => x.GetAlias(Constants.AliasName))
            .ReturnsAsync(() => new SearchAlias(Constants.AliasName, [indexName]));
        azureDocumentFactory.Setup(x=>x.Create(liveVacancy)).Returns(azureSearchDocuments);

        await sut.Handle(vacancyApprovedEvent);

        azureSearchHelper.Verify(x => x.UploadDocuments(It.Is<string>(i => i == indexName),
                azureSearchDocuments), Times.Once());
    }
}
