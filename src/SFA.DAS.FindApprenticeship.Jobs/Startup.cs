using System;
using System.IO;
using System.Net.Http;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NServiceBus;
using Polly;
using Polly.Extensions.Http;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.FindApprenticeship.Jobs;
using SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Application.Services;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Domain.ServiceCollectionExtensions;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
using SFA.DAS.NServiceBus.AzureFunction.Hosting;

[assembly: FunctionsStartup(typeof(Startup))]
namespace SFA.DAS.FindApprenticeship.Jobs;
public class Startup : FunctionsStartup
{
    private IConfiguration _configuration;

    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddHttpClient();
        builder.Services.AddLogging(logging =>
        {
            logging.AddConsole();
        });
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

        _configuration = config.Build();
        builder.Services.AddOptions();
        builder.Services.Configure<FindApprenticeshipJobsConfiguration>(_configuration.GetSection(nameof(FindApprenticeshipJobsConfiguration)));
        builder.Services.AddSingleton(cfg => cfg.GetService<IOptions<FindApprenticeshipJobsConfiguration>>().Value);

        builder.Services.AddSingleton(new FunctionEnvironment(configuration["EnvironmentName"]));

        builder.Services.AddTransient<IRecruitService, RecruitService>();
        builder.Services.AddTransient<IAzureSearchHelper, AzureSearchHelper>();
        builder.Services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        builder.Services.AddHttpClient<IAzureSearchApiClient, AzureSearchApiClient>();
        builder.Services.AddTransient<IRecruitIndexerJobHandler, RecruitIndexerJobHandler>();
        builder.Services.AddTransient<IIndexCleanupJobHandler, IndexCleanupJobHandler>();
        builder.Services.AddTransient<IVacancyUpdatedHandler, VacancyUpdatedHandler>();
        builder.Services.AddTransient<IDateTimeService, DateTimeService>();
        builder.Services.AddHttpClient<IRecruitApiClient, RecruitApiClient>
        (
            options => options.Timeout = TimeSpan.FromMinutes(30)
        )
        .SetHandlerLifetime(TimeSpan.FromMinutes(10))
        .AddPolicyHandler(HttpClientRetryPolicy());

        var logger = serviceProvider.GetService<ILoggerProvider>().CreateLogger(GetType().AssemblyQualifiedName);
        if (_configuration["NServiceBusConnectionString"] == "UseLearningEndpoint=true")
        {
            builder.Services.AddNServiceBus(logger, (options) =>
            {
                options.EndpointConfiguration = (endpoint) =>
                {
                    endpoint.UseTransport<LearningTransport>()
                        .StorageDirectory(_configuration.GetValue("UseLearningEndpointStorageDirectory", 
                        Path.Combine(Path.GetTempPath(), ".learningtransport")));
                    return endpoint;
                };
            });
        }
        else
        {
            builder.Services.AddNServiceBus(logger);
        }

        var webBuilder = builder.Services.AddWebJobs(_ => { });
        webBuilder.AddExecutionContextBinding();
        webBuilder.AddExtension(new NServiceBusExtensionConfigProvider());
    }

    private static IAsyncPolicy<HttpResponseMessage> HttpClientRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                retryAttempt)));
    }
}