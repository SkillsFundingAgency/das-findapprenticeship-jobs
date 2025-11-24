using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Reqnroll;
using SFA.DAS.FindApprenticeship.Jobs.AcceptanceTests.Infrastructure;
using SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Endpoints;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.AcceptanceTests.Steps;

[Binding]
public class RecruitIndexerSteps
{
    private readonly ScenarioContext _context;
    public RecruitIndexerSteps(ScenarioContext context)
    {
        _context = context;
    }

    [Given(@"I invoke the recruit indexer function")]
    public async Task GivenIInvokeTheRecruitIndexerFunction()
    {
        var recruitIndexerFunction = new RecruitIndexerTimerTrigger(It.IsAny<RecruitIndexerJobHandler>(), It.IsAny<ILogger<RecruitIndexerTimerTrigger>>());
        await recruitIndexerFunction.Run(It.IsAny<TimerInfo>());
    }

    [Given("I invoke the passing test")]
    public async Task GivenIInvokeThePassingTest()
    {
        Assert.Pass();
    }

    [When(@"I have vacancies")]
    public async Task WhenIHaveVacancies()
    {
        var recruitService = _context.Get<Mock<IOuterApiClient>>(ContextKeys.MockApiClient);
        await recruitService.Object.Get<GetLiveVacanciesApiResponse>(It.IsAny<GetLiveVacanciesApiRequest>());
        await recruitService.Object.Get<GetCivilServiceLiveVacanciesApiResponse>(It.IsAny<GetCivilServiceVacanciesApiRequest>());
    }

    [Then(@"they are added to the search index")]
    public void ThenTheyAreAddedToTheSearchIndex()
    {
        var azureSearchHelper = _context.Get<Mock<IAzureSearchHelper>>(ContextKeys.MockAzureSearchHelper);

        azureSearchHelper.Verify(x => x.DeleteIndex(It.IsAny<string>()), Times.Once());
        azureSearchHelper.Verify(x => x.CreateIndex(It.IsAny<string>()), Times.Once());
        azureSearchHelper.Verify(x => x.UploadDocuments(It.IsAny<string>(), It.IsAny<List<ApprenticeAzureSearchDocument>>()), Times.Once());
    }
}
