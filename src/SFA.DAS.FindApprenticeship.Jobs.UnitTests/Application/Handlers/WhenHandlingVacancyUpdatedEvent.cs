using AutoFixture.NUnit3;
using Azure;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Handlers;
public class WhenHandlingVacancyUpdatedEvent
{
    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_Is_Fetched_From_The_Index(
        VacancyUpdatedEvent vacancyUpdatedEvent,
        Response<ApprenticeAzureSearchDocument> document,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        VacancyUpdatedHandler sut)
    {
        azureSearchHelper.Setup(x => x.GetDocument(document.Value.VacancyReference)).ReturnsAsync(document);

        await sut.Handle(vacancyUpdatedEvent);

        azureSearchHelper.Verify(x => x.GetDocument($"REF{vacancyUpdatedEvent.VacancyReference}"), Times.Once());
    }

    [Test, MoqAutoData]
    public async Task Then_The_Updated_Vacancy_Is_Fetched_From_The_Api(
        VacancyUpdatedEvent vacancyUpdatedEvent,
        Response<ApprenticeAzureSearchDocument> document,
        LiveVacancy liveVacancy,
        [Frozen] Mock<IRecruitApiClient> recruitApiClient,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        VacancyUpdatedHandler sut)
    {
        azureSearchHelper.Setup(x => x.GetDocument(document.Value.VacancyReference)).ReturnsAsync(document);
        recruitApiClient.Setup(x => x.Get<GetLiveVacancyApiResponse>(new GetLiveVacancyApiRequest($"REF{vacancyUpdatedEvent.VacancyReference}"))).ReturnsAsync(liveVacancy);

        await sut.Handle(vacancyUpdatedEvent);

        recruitApiClient.Verify(x => x.Get<GetLiveVacancyApiResponse>(new GetLiveVacancyApiRequest($"REF{vacancyUpdatedEvent.VacancyReference}")), Times.Once());
    }


    [Test, MoqAutoData]
    public async Task Then_The_StartDate_Is_Changed_And_The_Vacancy_Is_Updated(
        VacancyUpdatedEvent vacancyUpdatedEvent,
        Response<ApprenticeAzureSearchDocument> document,
        LiveVacancy liveVacancy,
        [Frozen] Mock<IRecruitApiClient> recruitApiClient,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        VacancyUpdatedHandler sut)
    {
        vacancyUpdatedEvent.UpdateKind = LiveUpdateKind.StartDate;
        azureSearchHelper.Setup(x => x.GetDocument(document.Value.VacancyReference)).ReturnsAsync(document);
        azureSearchHelper.Setup(x => x.UploadDocuments(It.IsAny<IEnumerable<ApprenticeAzureSearchDocument>>())).Returns(Task.CompletedTask);
        recruitApiClient.Setup(x => x.Get<GetLiveVacancyApiResponse>(new GetLiveVacancyApiRequest($"REF{vacancyUpdatedEvent.VacancyReference}"))).ReturnsAsync(liveVacancy);

        var expectedDocument = document.Value;
        expectedDocument.StartDate = liveVacancy.StartDate;
        await sut.Handle(vacancyUpdatedEvent);

        using (new AssertionScope())
        {
            recruitApiClient.Verify(x => x.Get<LiveVacancy>(new GetLiveVacancyApiRequest($"REF{vacancyUpdatedEvent.VacancyReference}")), Times.Once());
            azureSearchHelper.Verify(x => x.UploadDocuments(Enumerable.Empty<ApprenticeAzureSearchDocument>().Append(document)), Times.Once());
            azureSearchHelper.Verify(x => x.UploadDocuments(Enumerable.Empty<ApprenticeAzureSearchDocument>().Append(expectedDocument)), Times.Once());
        }
    }

    [Test, MoqAutoData]
    public async Task Then_The_Closing_Date_Is_Changed_And_The_Vacancy_Is_Updated(
                VacancyUpdatedEvent vacancyUpdatedEvent,
        Response<ApprenticeAzureSearchDocument> document,
        LiveVacancy liveVacancy,
        [Frozen] Mock<IRecruitApiClient> recruitApiClient,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        VacancyUpdatedHandler sut)
    {
        vacancyUpdatedEvent.UpdateKind = LiveUpdateKind.ClosingDate;
        azureSearchHelper.Setup(x => x.GetDocument(document.Value.VacancyReference)).ReturnsAsync(document);
        azureSearchHelper.Setup(x => x.UploadDocuments(It.IsAny<IEnumerable<ApprenticeAzureSearchDocument>>())).Returns(Task.CompletedTask);
        recruitApiClient.Setup(x => x.Get<GetLiveVacancyApiResponse>(new GetLiveVacancyApiRequest($"REF{vacancyUpdatedEvent.VacancyReference}"))).ReturnsAsync(liveVacancy);

        var expectedDocument = document.Value;
        expectedDocument.ClosingDate = liveVacancy.ClosingDate;
        await sut.Handle(vacancyUpdatedEvent);

        using (new AssertionScope())
        {
            recruitApiClient.Verify(x => x.Get<GetLiveVacancyApiResponse>(new GetLiveVacancyApiRequest($"REF{vacancyUpdatedEvent.VacancyReference}")), Times.Once());
            azureSearchHelper.Verify(x => x.UploadDocuments(Enumerable.Empty<ApprenticeAzureSearchDocument>().Append(document)), Times.Once());
            azureSearchHelper.Verify(x => x.UploadDocuments(Enumerable.Empty<ApprenticeAzureSearchDocument>().Append(expectedDocument)), Times.Once());
        }
    }
}
