using Azure;
using Azure.Search.Documents.Indexes.Models;
using Esfa.Recruit.Vacancies.Client.Domain.Events;
using SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Handlers;

public class WhenHandlingVacancyClosedEvent
{
    [Test, MoqAutoData]
    public async Task If_No_Index_Is_Found_Then_The_Vacancy_Is_Still_Closed(
        SearchAlias searchAlias,
        VacancyClosedEvent vacancyClosedEvent,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        [Frozen] Mock<IFindApprenticeshipJobsService> findApprenticeshipJobsService,
        VacancyClosedHandler sut)
    {
        // arrange
        searchAlias.Indexes.Clear();
        azureSearchHelper.Setup(x => x.GetAlias(Constants.AliasName)).ReturnsAsync(searchAlias);
        
        // act
        await sut.Handle(vacancyClosedEvent);
        
        // assert
        findApprenticeshipJobsService.Verify(x => x.CloseVacancyEarly(vacancyClosedEvent.VacancyReference), Times.Once);
        azureSearchHelper.Verify(x => x.DeleteDocuments(It.IsAny<string>(), It.IsAny<IEnumerable<string>>()), Times.Never);
    }
    [Test, MoqAutoData]
    public async Task If_No_Alias_Is_Found_Then_The_Vacancy_Is_Still_Closed(
        VacancyClosedEvent vacancyClosedEvent,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        [Frozen] Mock<IFindApprenticeshipJobsService> findApprenticeshipJobsService,
        VacancyClosedHandler sut)
    {
        // arrange
        azureSearchHelper.Setup(x => x.GetAlias(Constants.AliasName))!.ReturnsAsync((SearchAlias)null!);
        
        // act
        await sut.Handle(vacancyClosedEvent);
        
        // assert
        findApprenticeshipJobsService.Verify(x => x.CloseVacancyEarly(vacancyClosedEvent.VacancyReference), Times.Once);
        azureSearchHelper.Verify(x => x.DeleteDocuments(It.IsAny<string>(), It.IsAny<IEnumerable<string>>()), Times.Never);
    }
    
    [Test, MoqAutoData]
    public async Task If_Index_Is_Found_Then_The_Vacancy_Is_Closed_And_Document_Is_Deleted(
        SearchAlias searchAlias,
        VacancyClosedEvent vacancyClosedEvent,
        Response<ApprenticeAzureSearchDocument> documentResponse,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        [Frozen] Mock<IFindApprenticeshipJobsService> findApprenticeshipJobsService,
        VacancyClosedHandler sut)
    {
        // arrange
        documentResponse.Value.OtherAddresses = null;
        azureSearchHelper.Setup(x => x.GetAlias(Constants.AliasName)).ReturnsAsync(searchAlias);
        azureSearchHelper.Setup(x => x.GetDocument(searchAlias.Indexes.First(), vacancyClosedEvent.VacancyReference.ToString())).ReturnsAsync(documentResponse);
        
        // act
        await sut.Handle(vacancyClosedEvent);
        
        // assert
        findApprenticeshipJobsService.Verify(x => x.CloseVacancyEarly(vacancyClosedEvent.VacancyReference), Times.Once);
        azureSearchHelper.Verify(x => x.DeleteDocuments(searchAlias.Indexes.First(), It.IsAny<IEnumerable<string>>()), Times.Once);
    }
    
    [Test, MoqAutoData]
    public async Task If_Index_Is_Found_Then_The_Multiple_Locations_Vacancy_Is_Closed_And_Documents_Are_Deleted(
        SearchAlias searchAlias,
        VacancyClosedEvent vacancyClosedEvent,
        Response<ApprenticeAzureSearchDocument> documentResponse,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        [Frozen] Mock<IFindApprenticeshipJobsService> findApprenticeshipJobsService,
        VacancyClosedHandler sut)
    {
        // arrange
        var id = vacancyClosedEvent.VacancyReference.ToString();
        IEnumerable<string>? capturedIds = null;
        azureSearchHelper.Setup(x => x.GetAlias(Constants.AliasName)).ReturnsAsync(searchAlias);
        azureSearchHelper.Setup(x => x.GetDocument(searchAlias.Indexes.First(), vacancyClosedEvent.VacancyReference.ToString())).ReturnsAsync(documentResponse);
        azureSearchHelper.Setup(x => x.DeleteDocuments(It.IsAny<string>(), It.IsAny<IEnumerable<string>>())).Callback((string _, IEnumerable<string> ids) => { capturedIds = ids; });
        
        // act
        await sut.Handle(vacancyClosedEvent);
        
        // assert
        findApprenticeshipJobsService.Verify(x => x.CloseVacancyEarly(vacancyClosedEvent.VacancyReference), Times.Once);
        var ids = capturedIds?.ToList();
        ids.Should().NotBeNullOrEmpty();
        ids.Should().HaveCount(documentResponse.Value.OtherAddresses!.Count + 1);
        ids.Should().Contain(id);
        ids.Should().Contain($"{id}-2");
        ids.Should().Contain($"{id}-3");
        ids.Should().Contain($"{id}-4");
    }

    [Test, MoqAutoData]
    public async Task Then_The_Event_Is_Ignored_If_No_Index_Is_Currently_Aliased(
        ILogger log,
        VacancyClosedEvent vacancyClosedEvent,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        [Frozen] Mock<IFindApprenticeshipJobsService> findApprenticeshipJobsService,
        VacancyClosedHandler sut)
    {
        azureSearchHelper.Setup(x => x.GetAlias(Constants.AliasName))
            .ReturnsAsync(() => null);

        await sut.Handle(vacancyClosedEvent);

        azureSearchHelper.Verify(x => x.DeleteDocuments(It.IsAny<string>(), It.IsAny<List<string>>()), Times.Never());
        findApprenticeshipJobsService.Verify(x => x.CloseVacancyEarly(vacancyClosedEvent.VacancyReference), Times.Once);
    }
}
