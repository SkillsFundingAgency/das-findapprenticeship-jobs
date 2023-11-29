using AutoFixture.NUnit3;
using Azure.Search.Documents.Indexes.Models;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Handlers;
public class WhenHandlingVacancyDeletedEvent
{

    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_Is_Deleted(
         VacancyDeletedEvent vacancyDeletedEvent,
        string aliasName,
        SearchAlias searchAlias,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        VacancyDeletedHandler sut)
    {
        azureSearchHelper.Setup(x => x.GetAlias(aliasName)).ReturnsAsync(searchAlias);
        azureSearchHelper.Setup(x => x.DeleteDocument(searchAlias.Indexes.FirstOrDefault(), $"VAC{vacancyDeletedEvent.VacancyId}")).Returns(Task.CompletedTask);

        await sut.Handle(vacancyDeletedEvent);

        azureSearchHelper.Verify(x => x.DeleteDocument(It.IsAny<string>(), $"VAC{vacancyDeletedEvent.VacancyId}"), Times.Once());
    }

    [Test, MoqAutoData]
    public async Task And_There_Is_No_Index_Found_Then_The_Document_Is_Not_Deleted(
        VacancyDeletedEvent vacancyDeletedEvent,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        VacancyDeletedHandler sut)
    {
        azureSearchHelper.Setup(x => x.GetAlias(It.IsAny<string>())).ReturnsAsync(() => null);
        azureSearchHelper.Setup(x => x.DeleteDocument(It.IsAny<string>(), $"VAC{vacancyDeletedEvent.VacancyId}")).Returns(Task.CompletedTask);

        await sut.Handle(vacancyDeletedEvent);

        azureSearchHelper.Verify(x => x.DeleteDocument(It.IsAny<string>(), $"VAC{vacancyDeletedEvent.VacancyId}"), Times.Never());
    }
}
