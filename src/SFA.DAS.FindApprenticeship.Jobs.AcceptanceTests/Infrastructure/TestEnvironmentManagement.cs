using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Azure;
using Azure.Search.Documents.Indexes.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;
using TechTalk.SpecFlow;

namespace SFA.DAS.FindApprenticeship.Jobs.AcceptanceTests.Infrastructure;

[Binding]
public class TestEnvironmentManagement
{
    private readonly ScenarioContext _context;
    private static HttpClient _staticClient;
    private Mock<IApiClient> _mockApiClient;
    private Mock<IAzureSearchHelper> _mockAzureSearchHelper;
    private static TestServer _server;

    public TestEnvironmentManagement(ScenarioContext context)
    {
        _context = context;
    }

    [BeforeScenario("MockApiClient")]
    public void Start()
    {
        _mockApiClient = new Mock<IApiClient>();
        _mockApiClient.Setup(x => x.Get<GetLiveVacanciesApiResponse>(It.IsAny<GetLiveVacanciesApiRequest>())).ReturnsAsync(TestDataValues.LiveVacanciesApiResponse);
        _mockAzureSearchHelper = new Mock<IAzureSearchHelper>();
        _mockAzureSearchHelper.Setup(x => x.GetIndex(It.IsAny<string>())).Returns(It.IsAny<Task<Response<SearchIndex>>>());
        _mockAzureSearchHelper.Setup(x => x.DeleteIndex(It.IsAny<string>())).Returns(Task.CompletedTask);
        _mockAzureSearchHelper.Setup(x => x.CreateIndex(It.IsAny<string>())).Returns(Task.CompletedTask);

        _server = new TestServer(new WebHostBuilder()
            .ConfigureTestServices(services => ConfigureTestServices(services, _mockApiClient, _mockAzureSearchHelper))
            .UseEnvironment(Environments.Development)
            .UseStartup<Startup>()
            .UseConfiguration(ConfigBuilder.GenerateConfiguration()));

        _staticClient = _server.CreateClient();

        _context.Set(_mockApiClient, ContextKeys.MockApiClient);
        _context.Set(_mockAzureSearchHelper, ContextKeys.MockAzureSearchHelper);
        _context.Set(_staticClient, ContextKeys.HttpClient);
    }

    [AfterScenario("MockApiClient")]
    public void StopEnvironment()
    {
        _server.Dispose();
        _staticClient?.Dispose();
    }

    private void ConfigureTestServices(IServiceCollection serviceCollection, Mock<IApiClient> mockRecruitService, Mock<IAzureSearchHelper> mockAzureSearchHelper)
    {
        foreach (var descriptor in serviceCollection.Where(
            d => d.ServiceType ==
                 typeof(IApiClient)).ToList())
        {
            serviceCollection.Remove(descriptor);
        }
        serviceCollection.AddSingleton(mockRecruitService);
        serviceCollection.AddSingleton(mockRecruitService.Object);
        serviceCollection.AddSingleton(mockAzureSearchHelper);
        serviceCollection.AddSingleton(mockAzureSearchHelper.Object);
    }
}
