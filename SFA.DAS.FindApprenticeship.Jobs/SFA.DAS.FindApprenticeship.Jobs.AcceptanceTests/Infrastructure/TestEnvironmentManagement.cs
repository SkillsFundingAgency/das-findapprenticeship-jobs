using System.Linq;
using System.Net.Http;
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
    private static TestServer _server;

    public TestEnvironmentManagement(ScenarioContext context)
    {
        _context = context;
    }

    [BeforeScenario("MockApiClient")]
    public void Start()
    {
        _mockApiClient = new Mock<IApiClient>();
        _mockApiClient.Setup(x => x.Get<GetLiveVacanciesApiResponse>(It.IsAny<GetLiveVacanciesRequest>())).ReturnsAsync(TestDataValues.LiveVacanciesApiResponse);

        _server = new TestServer(new WebHostBuilder()
            .ConfigureTestServices(services => ConfigureTestServices(services, _mockApiClient))
            .UseEnvironment(Environments.Development)
            .UseStartup<Startup>()
            .UseConfiguration(ConfigBuilder.GenerateConfiguration()));

        _staticClient = _server.CreateClient();

        _context.Set(_mockApiClient, ContextKeys.MockApiClient);
        _context.Set(_staticClient, ContextKeys.HttpClient);
    }

    [AfterScenario("MockApiClient")]
    public void StopEnvironment()
    {
        _server.Dispose();
        _staticClient?.Dispose();
    }

    private void ConfigureTestServices(IServiceCollection serviceCollection, Mock<IApiClient> mockRecruitService)
    {
        foreach (var descriptor in serviceCollection.Where(
            d => d.ServiceType ==
                 typeof(IApiClient)).ToList())
        {
            serviceCollection.Remove(descriptor);
        }
        serviceCollection.AddSingleton(mockRecruitService);
        serviceCollection.AddSingleton(mockRecruitService.Object);
    }
}
