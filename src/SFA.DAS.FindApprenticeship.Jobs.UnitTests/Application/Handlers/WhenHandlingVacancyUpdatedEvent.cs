//using AutoFixture.NUnit3;
//using Azure;
//using FluentAssertions.Execution;
//using Microsoft.Extensions.Logging;
//using Moq;
//using NUnit.Framework;
//using SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
//using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
//using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
//using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;
//using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;
//using SFA.DAS.Testing.AutoFixture;

//namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Handlers;
//public class WhenHandlingVacancyUpdatedEvent
//{
//    //TODO: uncomment once FAI-1020 is done
//    [Test, MoqAutoData]
//    public async Task Then_The_Vacancy_Is_Fetched_From_The_Index(
//        VacancyUpdatedEvent vacancyUpdatedEvent,
//        Response<ApprenticeAzureSearchDocument> document,
//        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
//        VacancyUpdatedHandler sut)
//    {
//        azureSearchHelper.Setup(x => x.GetDocument(It.IsAny<string>(), $"VAC{vacancyUpdatedEvent.VacancyReference}")).ReturnsAsync(document);

//        await sut.Handle(vacancyUpdatedEvent);

//        azureSearchHelper.Verify(x => x.GetDocument(It.IsAny<string>(), $"VAC{vacancyUpdatedEvent.VacancyReference}"), Times.Once());
//    }

//    [Test, MoqAutoData]
//    public async Task Then_The_Updated_Vacancy_Is_Fetched_From_The_Api(
//        VacancyUpdatedEvent vacancyUpdatedEvent,
//        Response<ApprenticeAzureSearchDocument> document,
//        Response<GetLiveVacancyApiResponse> liveVacancy,
//        [Frozen] Mock<IRecruitService> recruitService,
//        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
//        VacancyUpdatedHandler sut)
//    {
//        azureSearchHelper.Setup(x => x.GetDocument(It.IsAny<string>(), document.Value.VacancyReference)).ReturnsAsync(document);
//        recruitService.Setup(x => x.GetLiveVacancy(vacancyUpdatedEvent.VacancyId)).ReturnsAsync(liveVacancy);

//        await sut.Handle(vacancyUpdatedEvent);

//        recruitService.Verify(x => x.GetLiveVacancy(vacancyUpdatedEvent.VacancyId), Times.Once());
//    }
//}
