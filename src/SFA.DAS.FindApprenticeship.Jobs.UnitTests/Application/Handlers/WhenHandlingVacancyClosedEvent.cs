﻿using AutoFixture.NUnit3;
using Azure.Search.Documents.Indexes.Models;
using Esfa.Recruit.Vacancies.Client.Domain.Events;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Handlers;
public class WhenHandlingVacancyClosedEvent
{

    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_Is_Deleted_And_Closed_Early_Request_Made(
        VacancyClosedEvent vacancyClosedEvent,
        ILogger log,
        string aliasName,
        SearchAlias searchAlias,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        [Frozen] Mock<IFindApprenticeshipJobsService> findApprenticeshipJobsService,
        VacancyClosedHandler sut)
    {
        azureSearchHelper.Setup(x => x.GetAlias(aliasName)).ReturnsAsync(searchAlias);
        azureSearchHelper.Setup(x => x.DeleteDocument(searchAlias.Indexes.FirstOrDefault(), $"{vacancyClosedEvent.VacancyReference}")).Returns(Task.CompletedTask);

        await sut.Handle(vacancyClosedEvent);

        azureSearchHelper.Verify(x => x.DeleteDocument(It.IsAny<string>(), $"{vacancyClosedEvent.VacancyReference}"), Times.Once());
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
        azureSearchHelper.Setup(x => x.GetAlias(It.IsAny<string>())).ReturnsAsync(() => null);
        azureSearchHelper.Setup(x => x.DeleteDocument(It.IsAny<string>(), $"{vacancyClosedEvent.VacancyReference}")).Returns(Task.CompletedTask);

        await sut.Handle(vacancyClosedEvent);

        azureSearchHelper.Verify(x => x.DeleteDocument(It.IsAny<string>(), $"{vacancyClosedEvent.VacancyReference}"), Times.Never());
        findApprenticeshipJobsService.Verify(x=>x.CloseVacancyEarly(vacancyClosedEvent.VacancyReference), Times.Once);
    }
}
