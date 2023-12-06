using System.Text.Json;
using AutoFixture.NUnit3;
using Azure;
using Azure.Search.Documents.Indexes.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Handlers;
public class WhenHandlingVacancyUpdatedEvent
{
    [Test]
    [MoqInlineAutoData(LiveUpdateKind.StartDate)]
    [MoqInlineAutoData(LiveUpdateKind.ClosingDate)]
    [MoqInlineAutoData(LiveUpdateKind.StartDate | LiveUpdateKind.ClosingDate)]
    public async Task Then_The_Vacancy_Is_Updated_In_The_Index(
        LiveUpdateKind updateKind,
        ILogger log,
        VacancyUpdatedEvent vacancyUpdatedEvent,
        string indexName,
        Response<ApprenticeAzureSearchDocument> document,
        Response<GetLiveVacancyApiResponse> liveVacancy,
        [Frozen] Mock<IRecruitService> recruitService,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        VacancyUpdatedHandler sut)
    {
        vacancyUpdatedEvent.UpdateKind = updateKind;

        var originalDocument = JsonSerializer.Deserialize<ApprenticeAzureSearchDocument>(JsonSerializer.Serialize(document.Value));

        recruitService.Setup(x => x.GetLiveVacancy(vacancyUpdatedEvent.VacancyReference)).ReturnsAsync(liveVacancy);
        azureSearchHelper.Setup(x => x.GetDocument(indexName, $"VAC{vacancyUpdatedEvent.VacancyReference}")).ReturnsAsync(document);
        azureSearchHelper.Setup(x => x.GetAlias(Constants.AliasName))
            .ReturnsAsync(() => new SearchAlias(Constants.AliasName, new[] { indexName }));

        await sut.Handle(vacancyUpdatedEvent, log);

        azureSearchHelper.Verify(x => x.UploadDocuments(It.Is<string>(i => i == indexName),
            It.Is<IEnumerable<ApprenticeAzureSearchDocument>>(d => AssertDocumentProperties(d, originalDocument, liveVacancy.Value, updateKind) )),
            Times.Once());
    }

    private bool AssertDocumentProperties(IEnumerable<ApprenticeAzureSearchDocument> updatedDocuments, ApprenticeAzureSearchDocument originalDocument, GetLiveVacancyApiResponse liveVacancyValue, LiveUpdateKind kind)
    {
        var updatedDocument = updatedDocuments.Single();

        var expectedStartDate = (kind.HasFlag(LiveUpdateKind.StartDate)
            ? liveVacancyValue.StartDate
            : originalDocument.StartDate);

        var expectedClosingDate = (kind.HasFlag(LiveUpdateKind.ClosingDate)
            ? liveVacancyValue.ClosingDate
            : originalDocument.ClosingDate);

        return updatedDocument.StartDate == expectedStartDate && updatedDocument.ClosingDate == expectedClosingDate;
    }

    [Test, MoqAutoData]
    public async Task Then_The_Event_Is_Ignored_If_No_Index_Is_Currently_Aliased(
        ILogger log,
        VacancyUpdatedEvent vacancyUpdatedEvent,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        VacancyUpdatedHandler sut)
    {
        azureSearchHelper.Setup(x => x.GetAlias(Constants.AliasName))
            .ReturnsAsync(() => null);

        await sut.Handle(vacancyUpdatedEvent, log);

        azureSearchHelper.Verify(x => x.UploadDocuments(It.IsAny<string>(), It.IsAny<IEnumerable<ApprenticeAzureSearchDocument>>()),
            Times.Never());
    }
}
