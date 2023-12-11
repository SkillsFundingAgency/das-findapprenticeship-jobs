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

[TestFixture]
public class WhenHandlingVacancyApprovedEvent
{
    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_Is_Uploaded_To_The_Index(
        ILogger log,
        VacancyApprovedEvent vacancyApprovedEvent,
        string indexName,
        int programmeId,
        Response<ApprenticeAzureSearchDocument> document,
        Response<GetLiveVacancyApiResponse> liveVacancy,
        [Frozen] Mock<IRecruitService> recruitService,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        VacancyApprovedHandler sut)
    {
        liveVacancy.Value.StandardLarsCode = programmeId;
        liveVacancy.Value.ProgrammeType = "Standard";

        recruitService.Setup(x => x.GetLiveVacancy(vacancyApprovedEvent.VacancyReference)).ReturnsAsync(liveVacancy);
        azureSearchHelper.Setup(x => x.GetDocument(indexName, $"VAC{vacancyApprovedEvent.VacancyReference}")).ReturnsAsync(document);
        azureSearchHelper.Setup(x => x.GetAlias(Constants.AliasName))
            .ReturnsAsync(() => new SearchAlias(Constants.AliasName, new[] { indexName }));

        await sut.Handle(vacancyApprovedEvent, log);

        azureSearchHelper.Verify(x => x.UploadDocuments(It.Is<string>(i => i == indexName),
                It.Is<IEnumerable<ApprenticeAzureSearchDocument>>(d =>
                    d.Single().VacancyReference == $"VAC{liveVacancy.Value.VacancyReference}")),
            Times.Once());
    }

    [Test, MoqAutoData]
    public async Task Then_The_Event_Is_Ignored_If_No_Index_Is_Currently_Aliased(
        ILogger log,
        VacancyApprovedEvent vacancyApprovedEvent,
        [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
        VacancyApprovedHandler sut)
    {
        azureSearchHelper.Setup(x => x.GetAlias(Constants.AliasName))
            .ReturnsAsync(() => null);

        await sut.Handle(vacancyApprovedEvent, log);

        azureSearchHelper.Verify(x => x.UploadDocuments(It.IsAny<string>(), It.IsAny<IEnumerable<ApprenticeAzureSearchDocument>>()),
            Times.Never());
    }
}
