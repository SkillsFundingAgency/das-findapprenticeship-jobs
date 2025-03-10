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
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Handlers;
public class WhenHandlingVacancyUpdatedEvent
{
    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_Is_Updated_In_The_Index(
        ILogger log,
        List<Address> otherAddresses,
        LiveVacancyUpdatedEvent vacancyUpdatedEvent,
        string indexName,
        Response<ApprenticeAzureSearchDocument> document,
        Response<GetLiveVacancyApiResponse> liveVacancy,
        [Frozen] Mock<IFindApprenticeshipJobsService> findApprenticeshipJobsService,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        VacancyUpdatedHandler sut)
    {
        liveVacancy.Value.OtherAddresses = otherAddresses;

        findApprenticeshipJobsService.Setup(x => x.GetLiveVacancy(vacancyUpdatedEvent.VacancyReference.ToString())).ReturnsAsync(liveVacancy);
        azureSearchHelper.Setup(x => x.GetDocument(indexName, $"{vacancyUpdatedEvent.VacancyReference}")).ReturnsAsync(document);
        azureSearchHelper.Setup(x => x.GetAlias(Constants.AliasName))
            .ReturnsAsync(() => new SearchAlias(Constants.AliasName, new[] { indexName }));

        await sut.Handle(vacancyUpdatedEvent);

        azureSearchHelper.Verify(x => x.UploadDocuments(It.Is<string>(i => i == indexName),
            It.Is<IEnumerable<ApprenticeAzureSearchDocument>>(d => AssertDocumentProperties(d, liveVacancy.Value) )),
            Times.Once());
    }

    private static bool AssertDocumentProperties(IEnumerable<ApprenticeAzureSearchDocument> updatedDocuments, LiveVacancy liveVacancy)
    {
        var updatedDocument = updatedDocuments.FirstOrDefault()!;

        return updatedDocument.StartDate == liveVacancy.StartDate && updatedDocument.ClosingDate == liveVacancy.ClosingDate;
    }

    [Test, MoqAutoData]
    public async Task Then_The_Event_Is_Ignored_If_No_Index_Is_Currently_Aliased(
        ILogger log,
        LiveVacancyUpdatedEvent vacancyUpdatedEvent,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        VacancyUpdatedHandler sut)
    {
        azureSearchHelper.Setup(x => x.GetAlias(Constants.AliasName))
            .ReturnsAsync(() => null);

        await sut.Handle(vacancyUpdatedEvent);

        azureSearchHelper.Verify(x => x.UploadDocuments(It.IsAny<string>(), It.IsAny<IEnumerable<ApprenticeAzureSearchDocument>>()),
            Times.Never());
    }

    [Test, MoqAutoData]
    public async Task Then_OtherAddresses_Are_Null_The_Vacancy_Is_Updated_In_The_Index(
        ILogger log,
        LiveVacancyUpdatedEvent vacancyUpdatedEvent,
        string indexName,
        Response<ApprenticeAzureSearchDocument> document,
        Response<GetLiveVacancyApiResponse> liveVacancy,
        [Frozen] Mock<IFindApprenticeshipJobsService> findApprenticeshipJobsService,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        VacancyUpdatedHandler sut)
    {
        liveVacancy.Value.EmploymentLocations = [];

        findApprenticeshipJobsService.Setup(x => x.GetLiveVacancy(vacancyUpdatedEvent.VacancyReference.ToString())).ReturnsAsync(liveVacancy);
        azureSearchHelper.Setup(x => x.GetDocument(indexName, $"{vacancyUpdatedEvent.VacancyReference}")).ReturnsAsync(document);
        azureSearchHelper.Setup(x => x.GetAlias(Constants.AliasName))
            .ReturnsAsync(() => new SearchAlias(Constants.AliasName, new[] { indexName }));

        await sut.Handle(vacancyUpdatedEvent);

        azureSearchHelper.Verify(x => x.UploadDocuments(It.Is<string>(i => i == indexName),
                It.Is<IEnumerable<ApprenticeAzureSearchDocument>>(d => AssertDocumentProperties(d, liveVacancy.Value))),
            Times.Exactly(1));
    }
}
