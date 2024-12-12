using AutoFixture.NUnit3;
using Azure;
using Azure.Search.Documents.Indexes.Models;
using Esfa.Recruit.Vacancies.Client.Domain.Events;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Handlers;
public class WhenHandlingVacancyClosedEvent
{
    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_Is_Deleted_And_Closed_Early_Request_Made(
        Response<ApprenticeAzureSearchDocument> searchDocument,
        VacancyClosedEvent vacancyClosedEvent,
        ILogger log,
        SearchAlias searchAlias,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        [Frozen] Mock<IFindApprenticeshipJobsService> findApprenticeshipJobsService,
        VacancyClosedHandler sut)
    {
        var indexName = searchAlias.Indexes.FirstOrDefault()!;
        var vacancyReferenceId = $"{vacancyClosedEvent.VacancyReference}";
        var vacancyIds = new List<string>
        {
            vacancyReferenceId
        };

        var counter = 1;
        foreach (var azureSearchDocumentKey in searchDocument.Value.OtherAddresses.Select(_ => $"{searchDocument.Value.Id}-{counter}"))
        {
            vacancyIds.Add(azureSearchDocumentKey);
            counter++;
        }

        azureSearchHelper.Setup(x => x.GetAlias(Jobs.Domain.Constants.AliasName)).ReturnsAsync(searchAlias);
        azureSearchHelper.Setup(x => x.GetDocument(indexName, vacancyReferenceId)).ReturnsAsync(searchDocument);
        azureSearchHelper.Setup(x => x.DeleteDocuments(indexName, vacancyIds)).Returns(Task.CompletedTask);

        await sut.Handle(vacancyClosedEvent);

        azureSearchHelper.Verify(x => x.DeleteDocuments(indexName, vacancyIds), Times.Once());
        findApprenticeshipJobsService.Verify(x=>x.CloseVacancyEarly(vacancyClosedEvent.VacancyReference), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task And_There_Is_No_Index_Found_Then_The_Document_Is_Not_Deleted_But_Close_Request_Still_Made(
        VacancyClosedEvent vacancyClosedEvent,
        ILogger log,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        [Frozen] Mock<IFindApprenticeshipJobsService> findApprenticeshipJobsService,
        VacancyClosedHandler sut)
    {
        var vacancyIds = new List<string>
        {
            $"{vacancyClosedEvent.VacancyReference}"
        };

        azureSearchHelper.Setup(x => x.GetAlias(It.IsAny<string>())).ReturnsAsync(() => null);
        azureSearchHelper.Setup(x => x.DeleteDocuments(It.IsAny<string>(), vacancyIds)).Returns(Task.CompletedTask);

        await sut.Handle(vacancyClosedEvent);

        azureSearchHelper.Verify(x => x.DeleteDocuments(It.IsAny<string>(), vacancyIds), Times.Never());
        findApprenticeshipJobsService.Verify(x=>x.CloseVacancyEarly(vacancyClosedEvent.VacancyReference), Times.Once);
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
