using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.AcceptanceTests.Infrastructure;
public class TestServiceProvider : IServiceProvider
{
    private readonly IServiceProvider _serviceProvider;

    public TestServiceProvider()
    {
        var serviceCollection = new ServiceCollection();
        var configuration = ConfigBuilder.GenerateConfiguration();

        var serviceProviderBuilder = new ServiceProviderBuilder(new LoggerFactory(), configuration)
        {
            ServiceCollection = serviceCollection
        };

        var recruitService = new Mock<IRecruitService>();
        recruitService.Setup(x => x.GetLiveVacancies(It.Is<int>(i => i.Equals(TestDataValues.PageNo)), It.Is<int>(i => i.Equals(TestDataValues.PageSize))))
            .ReturnsAsync(TestDataValues.LiveVacancies);

        serviceCollection.AddSingleton(recruitService.Object);

        _serviceProvider = serviceProviderBuilder.Build();
    }
    public object GetService(Type serviceType)
    {
        return _serviceProvider.GetService(serviceType);
    }
}
