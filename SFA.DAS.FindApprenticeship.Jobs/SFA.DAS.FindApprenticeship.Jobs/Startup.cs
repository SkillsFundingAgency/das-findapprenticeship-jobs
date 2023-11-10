using System.IO;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.FindApprenticeship.Jobs;
using SFA.DAS.FindApprenticeship.Jobs.Application.Services;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;

[assembly: FunctionsStartup(typeof(Startup))]
namespace SFA.DAS.FindApprenticeship.Jobs;
public class Startup : FunctionsStartup
{
    private IConfiguration _configuration;

    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddHttpClient();

        var serviceProvider = builder.Services.BuildServiceProvider();
        var configuration = serviceProvider.GetService<IConfiguration>();

        var config = new ConfigurationBuilder()
            .AddConfiguration(configuration)
            .SetBasePath(Directory.GetCurrentDirectory())
#if DEBUG 
            .AddJsonFile("local.settings.json", true)
            .AddJsonFile("local.settings.Development.json", true)
#endif
            .AddEnvironmentVariables();

        if (!configuration["EnvironmentName"].Equals("DEV", System.StringComparison.CurrentCultureIgnoreCase))
        {
            config.AddAzureTableStorage(options =>
            {
                options.ConfigurationKeys = configuration["ConfigNames"].Split(",");
                options.StorageConnectionString = configuration["ConfigurationStorageConnectionString"];
                options.EnvironmentName = configuration["EnvironmentName"];
                options.PreFixConfigurationKeys = false;
            }
            );
        }

        _configuration = config.Build();
        builder.Services.AddOptions();
        builder.Services.Configure<FindApprenticeshipJobsConfiguration>(_configuration.GetSection(nameof(FindApprenticeshipJobsConfiguration)));
        builder.Services.AddSingleton(cfg => cfg.GetService<IOptions<FindApprenticeshipJobsConfiguration>>().Value);

        builder.Services.AddSingleton(new FunctionEnvironment(configuration["EnvironmentName"]));

        builder.Services.AddTransient<IRecruitService, RecruitService>();
        builder.Services.AddTransient<IAzureSearchIndexService, AzureSearchIndexService>();
        builder.Services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        builder.Services.AddHttpClient<IAzureSearchApiClient, AzureSearchApiClient>();
        builder.Services.AddHttpClient<IRecruitApiClient, RecruitApiClient>();

        builder.Services.BuildServiceProvider();
    }
}
