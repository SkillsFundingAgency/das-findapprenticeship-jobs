//using AutoFixture.NUnit3;
//using Azure;
//using Azure.Search.Documents.Indexes.Models;
//using Microsoft.Extensions.Logging;
//using Moq;
//using NUnit.Framework;
//using SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
//using SFA.DAS.FindApprenticeship.Jobs.Domain;
//using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
//using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
//using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;
//using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;
//using SFA.DAS.Testing.AutoFixture;

//namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Handlers;
//public class WhenHandlingVacancyApprovedEvent
//{
//    [Test, MoqAutoData]
//    public async Task Then_The_Vacancy_Is_Fetched_From_The_Api(
//        string aliasName,
//        SearchAlias searchAlias,
//        VacancyApprovedEvent vacancyApprovedEvent,
//        Response<GetLiveVacancyApiResponse> liveVacancy,
//        ILogger log,
//        [Frozen] Mock<IRecruitService> recruitService,
//        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
//        VacancyApprovedHandler sut)
//    {
//        liveVacancy.Value.LiveVacancy = TestData.LiveVacancies[0];
//        recruitService.Setup(x => x.GetLiveVacancy(vacancyApprovedEvent.VacancyReference)).ReturnsAsync(liveVacancy);
//        azureSearchHelper.Setup(x => x.GetAlias(aliasName)).ReturnsAsync(searchAlias);
//        azureSearchHelper.Setup(x => x.UploadDocuments(It.IsAny<string>(), It.IsAny<List<ApprenticeAzureSearchDocument>>())).Returns(Task.CompletedTask);

//        await sut.Handle(vacancyApprovedEvent, log);

//        recruitService.Verify(x => x.GetLiveVacancy(vacancyApprovedEvent.VacancyReference), Times.Once);
//    }

//    [Test, MoqAutoData]
//    public async Task Then_The_Alias_Is_Fetched(
//        SearchAlias searchAlias,
//        VacancyApprovedEvent vacancyApprovedEvent,
//        Response<GetLiveVacancyApiResponse> liveVacancy,
//        ILogger log,
//        [Frozen] Mock<IRecruitService> recruitService,
//        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
//        VacancyApprovedHandler sut)
//    {
//        liveVacancy.Value.LiveVacancy = TestData.LiveVacancies[0];
//        recruitService.Setup(x => x.GetLiveVacancy(vacancyApprovedEvent.VacancyReference)).ReturnsAsync(liveVacancy);
//        azureSearchHelper.Setup(x => x.GetAlias(It.IsAny<string>())).ReturnsAsync(searchAlias);
//        azureSearchHelper.Setup(x => x.UploadDocuments(It.IsAny<string>(), It.IsAny<List<ApprenticeAzureSearchDocument>>())).Returns(Task.CompletedTask);

//        await sut.Handle(vacancyApprovedEvent, log);

//        azureSearchHelper.Verify(x => x.GetAlias(Constants.AliasName), Times.Once);
//    }

//    [Test, MoqAutoData]
//    public async Task And_Vacancy_Is_Not_Null_And_IndexName_Is_Not_Null_Then_Vacancy_Is_Uploaded_To_Index(
//        string aliasName,
//        SearchAlias searchAlias,
//        VacancyApprovedEvent vacancyApprovedEvent,
//        Response<GetLiveVacancyApiResponse> liveVacancy,
//        ILogger log,
//        [Frozen] Mock<IRecruitService> recruitService,
//        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
//        VacancyApprovedHandler sut)
//    {
//        liveVacancy.Value.LiveVacancy = TestData.LiveVacancies[0];
//        recruitService.Setup(x => x.GetLiveVacancy(vacancyApprovedEvent.VacancyReference)).ReturnsAsync(liveVacancy);
//        azureSearchHelper.Setup(x => x.GetAlias(aliasName)).ReturnsAsync(searchAlias);
//        azureSearchHelper.Setup(x => x.UploadDocuments(It.IsAny<string>(), It.IsAny<List<ApprenticeAzureSearchDocument>>())).Returns(Task.CompletedTask);

//        await sut.Handle(vacancyApprovedEvent, log);

//        azureSearchHelper.Verify(x => x.UploadDocuments(It.IsAny<string>(), It.IsAny<IEnumerable<ApprenticeAzureSearchDocument>>()), Times.Once);
//    }

//    [Test, MoqAutoData]
//    public async Task And_Vacancy_Is_Null_Then_Vacancy_Is_Not_Uploaded(string aliasName,
//        SearchAlias searchAlias,
//        VacancyApprovedEvent vacancyApprovedEvent,
//        ILogger log,
//        [Frozen] Mock<IRecruitService> recruitService,
//        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
//        VacancyApprovedHandler sut)
//    {
//        recruitService.Setup(x => x.GetLiveVacancy(vacancyApprovedEvent.VacancyReference)).ReturnsAsync(() => null);
//        azureSearchHelper.Setup(x => x.GetAlias(aliasName)).ReturnsAsync(searchAlias);

//        await sut.Handle(vacancyApprovedEvent, log);

//        azureSearchHelper.Verify(x => x.UploadDocuments(It.IsAny<string>(), It.IsAny<IEnumerable<ApprenticeAzureSearchDocument>>()), Times.Never);
//    }
//}
