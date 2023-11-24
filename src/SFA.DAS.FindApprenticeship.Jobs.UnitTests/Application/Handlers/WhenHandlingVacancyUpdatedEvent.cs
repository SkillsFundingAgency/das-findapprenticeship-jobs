using AutoFixture.NUnit3;
using Azure;
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
        azureSearchHelper.Setup(x => x.GetDocument(It.IsAny<string>(), $"VAC{vacancyUpdatedEvent.VacancyReference}")).ReturnsAsync(document);

        await sut.Handle(vacancyUpdatedEvent);

        azureSearchHelper.Verify(x => x.GetDocument(It.IsAny<string>(), $"VAC{vacancyUpdatedEvent.VacancyReference}"), Times.Once());
    }

    [Test, MoqAutoData]
    public async Task Then_The_Updated_Vacancy_Is_Fetched_From_The_Api(
        VacancyUpdatedEvent vacancyUpdatedEvent,
        Response<ApprenticeAzureSearchDocument> document,
        Response<GetLiveVacancyApiResponse> liveVacancy,
        [Frozen] Mock<IRecruitService> recruitService,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        VacancyUpdatedHandler sut)
    {
        azureSearchHelper.Setup(x => x.GetDocument(It.IsAny<string>(), document.Value.VacancyReference)).ReturnsAsync(document);
        recruitService.Setup(x => x.GetLiveVacancy(vacancyUpdatedEvent.VacancyId)).ReturnsAsync(liveVacancy);

        await sut.Handle(vacancyUpdatedEvent);

        recruitService.Verify(x => x.GetLiveVacancy(vacancyUpdatedEvent.VacancyId), Times.Once());
    }


    //[Test, MoqAutoData]
    //public async Task Then_The_StartDate_Is_Changed_And_The_Vacancy_Is_Updated(
    //    VacancyUpdatedEvent vacancyUpdatedEvent,
    //    Response<ApprenticeAzureSearchDocument> originalVacancy,
    //    Response<GetLiveVacancyApiResponse> updatedVacancy,
    //    [Frozen] Mock<IRecruitService> recruitService,
    //    [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
    //    VacancyUpdatedHandler sut)
    //{
    //    // initialise test data
    //    updatedVacancy.Value.LiveVacancy = TestData.LiveVacancies[0];
    //    var liveVacancyDocument = (ApprenticeAzureSearchDocument)updatedVacancy.Value.LiveVacancy;
    //    //originalVacancy.Value = (ApprenticeAzureSearchDocument)updatedVacancy;

    //    // update vacancy so it is different
    //    updatedVacancy.Value.LiveVacancy.StartDate = DateTime.Now;
    //    vacancyUpdatedEvent.UpdateKind = LiveUpdateKind.StartDate;

    //    azureSearchHelper.Setup(x => x.GetDocument(It.IsAny<string>())).ReturnsAsync(originalVacancy);
    //    azureSearchHelper.Setup(x => x.UploadDocuments(It.IsAny<IEnumerable<ApprenticeAzureSearchDocument>>())).Returns(Task.CompletedTask);
    //    recruitService.Setup(x => x.GetLiveVacancy(vacancyUpdatedEvent.VacancyId)).ReturnsAsync(updatedVacancy);

    //    await sut.Handle(vacancyUpdatedEvent);

    //    using (new AssertionScope())
    //    {
    //        //recruitService.Verify(x => x.GetLiveVacancy(vacancyUpdatedEvent.VacancyId), Times.Once());
    //        //azureSearchHelper.Verify(x => x.UploadDocuments(Enumerable.Empty<ApprenticeAzureSearchDocument>().Append(document)), Times.Never());
    //        azureSearchHelper.Verify(x => x.UploadDocuments(Enumerable.Empty<ApprenticeAzureSearchDocument>().Append(expectedDocument)), Times.Once());
    //    }
    //}

    //[Test, MoqAutoData]
    //public async Task Then_The_Closing_Date_Is_Changed_And_The_Vacancy_Is_Updated(
    //    VacancyUpdatedEvent vacancyUpdatedEvent,
    //    Response<ApprenticeAzureSearchDocument> document,
    //    Response<GetLiveVacancyApiResponse> liveVacancy,
    //    [Frozen] Mock<IRecruitService> recruitService,
    //    [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
    //    VacancyUpdatedHandler sut)
    //{
    //    CreateLiveVacancyApiResponse(liveVacancy.Value.LiveVacancy, document.Value);
    //    vacancyUpdatedEvent.UpdateKind = LiveUpdateKind.ClosingDate;
    //    azureSearchHelper.Setup(x => x.GetDocument(document.Value.VacancyReference)).ReturnsAsync(document);
    //    azureSearchHelper.Setup(x => x.UploadDocuments(It.IsAny<IEnumerable<ApprenticeAzureSearchDocument>>())).Returns(Task.CompletedTask);
    //    recruitService.Setup(x => x.GetLiveVacancy(vacancyUpdatedEvent.VacancyId)).ReturnsAsync(liveVacancy);

    //    var expectedDocument = document.Value;
    //    expectedDocument.ClosingDate = liveVacancy.Value.LiveVacancy.ClosingDate;
    //    await sut.Handle(vacancyUpdatedEvent);

    //    using (new AssertionScope())
    //    {
    //        recruitService.Verify(x => x.GetLiveVacancy(vacancyUpdatedEvent.VacancyId), Times.Once());
    //        azureSearchHelper.Verify(x => x.UploadDocuments(Enumerable.Empty<ApprenticeAzureSearchDocument>().Append(document)), Times.Once());
    //        azureSearchHelper.Verify(x => x.UploadDocuments(Enumerable.Empty<ApprenticeAzureSearchDocument>().Append(expectedDocument)), Times.Once());
    //    }
    //}

    //private LiveVacancy CreateLiveVacancyApiResponse(LiveVacancy liveVacancyApiResponse, ApprenticeAzureSearchDocument liveVacancyDocument)
    //{
    //    liveVacancyApiResponse.Description = liveVacancyDocument.Description;
    //    liveVacancyApiResponse.Route = liveVacancyDocument.Route;
    //    liveVacancyApiResponse.EmployerName = liveVacancyDocument.EmployerName;
    //    liveVacancyApiResponse.Wage.WeeklyHours = liveVacancyDocument.HoursPerWeek;
    //    liveVacancyApiResponse.ProviderName = liveVacancyDocument.ProviderName;
    //    liveVacancyApiResponse.StartDate = liveVacancyDocument.StartDate;
    //    liveVacancyApiResponse.LiveDate = liveVacancyDocument.PostedDate;
    //    liveVacancyApiResponse.ClosingDate = liveVacancyDocument.ClosingDate;
    //    liveVacancyApiResponse.VacancyTitle = liveVacancyDocument.Title;
    //    liveVacancyApiResponse.ProviderId = liveVacancyDocument.Ukprn;
    //    liveVacancyApiResponse.Wage.WeeklyHours = li
    //}

    //Description = source.Description,
    //        Route = source.Route,
    //        EmployerName = source.EmployerName,
    //        HoursPerWeek = (long) source.Wage!.WeeklyHours,
    //        ProviderName = source.ProviderName,
    //        StartDate = source.StartDate,
    //        PostedDate = source.LiveDate,
    //        ClosingDate = source.ClosingDate,
    //        Title = source.VacancyTitle,
    //Ukprn = source.ProviderId,
    //        VacancyReference = $"VAC{source.VacancyId}",
    //Wage = (WageAzureSearchDocument)source.Wage,
    //        Course = (CourseAzureSearchDocument)source,
    //Address = (AddressAzureSearchDocument)source.EmployerLocation,
    //Location = GeographyPoint.Create(source.EmployerLocation!.Latitude, source.EmployerLocation!.Longitude),
    //// Use for 'vacancies' index:
    //        NumberOfPositions = 2
}
