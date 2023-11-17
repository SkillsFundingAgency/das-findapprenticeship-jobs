using System;
using System.IO;
using System.Net.Http;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.FindApprenticeship.Jobs;
using SFA.DAS.FindApprenticeship.Jobs.Application.Services;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace SFA.DAS.FindApprenticeship.Jobs;
public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var wbBuilder = builder.Services.AddWebJobs(x => { return; });
        wbBuilder.AddExecutionContextBinding();
        wbBuilder.AddDependencyInjection<ServiceProviderBuilder>();
    }
}

public class ServiceProviderBuilder : IServiceProviderBuilder
{
    public ServiceCollection ServiceCollection { get; set; }

    private readonly ILoggerFactory _loggerFactory;
    public IConfiguration Configuration { get; }

    public ServiceProviderBuilder(ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        _loggerFactory = loggerFactory;

        var config = new ConfigurationBuilder()
            .AddConfiguration(configuration)
            .SetBasePath(Directory.GetCurrentDirectory())
#if DEBUG
            .AddJsonFile("local.settings.json", true)
            .AddJsonFile("local.settings.Development.json", true)
#endif
            .AddEnvironmentVariables();

        if (!configuration["EnvironmentName"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
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

        Configuration = config.Build();
    }

    public IServiceProvider Build()
    {
        var services = ServiceCollection ?? new ServiceCollection();
        services.AddHttpClient();
        services.AddOptions();

        services.Configure<FindApprenticeshipJobsConfiguration>(Configuration.GetSection(nameof(FindApprenticeshipJobsConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<FindApprenticeshipJobsConfiguration>>().Value);
        services.AddSingleton(new FunctionEnvironment(Configuration["EnvironmentName"]));

        services.AddHttpClient<IAzureSearchApiClient, AzureSearchApiClient>();
        services.AddHttpClient<IRecruitApiClient, RecruitApiClient>
        (
            options => options.Timeout = TimeSpan.FromMinutes(30)
        )
        .SetHandlerLifetime(TimeSpan.FromMinutes(10))
        .AddPolicyHandler(HttpClientRetryPolicy());
        services.AddTransient<IAzureSearchHelper, AzureSearchHelper>();
        services.AddTransient<IRecruitService, RecruitService>();
        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();

        return services.BuildServiceProvider();
    }

    private static IAsyncPolicy<HttpResponseMessage> HttpClientRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                retryAttempt)));
    }
}
