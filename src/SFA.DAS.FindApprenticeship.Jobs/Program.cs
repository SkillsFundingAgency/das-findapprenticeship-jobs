using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.Encoding;
using SFA.DAS.FindApprenticeship.Jobs.Application;
using SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Application.Services;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
using SFA.DAS.FindApprenticeship.Jobs.StartupExtensions;

[assembly: NServiceBusTriggerFunction("SFA.DAS.FindApprenticeship.Jobs")]
var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration(builder => builder.BuildConfiguration())
    .ConfigureNServiceBus()
    .ConfigureServices((context, services) =>
    {
        services.AddLogging(builder =>
            {
                builder.AddFilter<ApplicationInsightsLoggerProvider>(string.Empty, LogLevel.Information);
                builder.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft", LogLevel.Information);

                builder.AddFilter(typeof(Program).Namespace, LogLevel.Information);
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddConsole();

            }
            );

        var configuration = context.Configuration;

        services.Replace(ServiceDescriptor.Singleton(typeof(IConfiguration), configuration));
        services.AddOptions();

        services.Configure<FindApprenticeshipJobsConfiguration>(configuration.GetSection(nameof(FindApprenticeshipJobsConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<FindApprenticeshipJobsConfiguration>>().Value);
        
        // Configure the DAS Encoding service
        var dasEncodingConfig = new EncodingConfig { Encodings = [] };
        context.Configuration.GetSection(nameof(dasEncodingConfig.Encodings)).Bind(dasEncodingConfig.Encodings);
        services.AddSingleton(dasEncodingConfig);
        services.AddSingleton<IEncodingService, EncodingService>();

        var environmentName = configuration["Values:EnvironmentName"] ?? configuration["EnvironmentName"];
        services.AddSingleton(new FunctionEnvironment(environmentName));

        services.AddTransient<IApprenticeAzureSearchDocumentFactory, ApprenticeAzureSearchDocumentFactory>();
        services.AddTransient<INhsAzureSearchDocumentFactory, NhsAzureSearchDocumentFactory>();
        services.AddTransient<IFindApprenticeshipJobsService, FindApprenticeshipJobsService>();
        services.AddTransient<IAzureSearchHelper, AzureSearchHelper>();
        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        services.AddTransient<IRecruitIndexerJobHandler, RecruitIndexerJobHandler>();
        services.AddTransient<IIndexCleanupJobHandler, IndexCleanupJobHandler>();
        services.AddTransient<IVacancyUpdatedHandler, VacancyUpdatedHandler>();
        services.AddTransient<IVacancyClosedHandler, VacancyClosedHandler>();
        services.AddTransient<IVacancyApprovedHandler, VacancyApprovedHandler>();
        services.AddTransient<IVacancyClosingSoonHandler, VacancyClosingSoonHandler>();
        services.AddTransient<ISendApplicationReminderHandler, SendApplicationReminderHandler>();
        services.AddTransient<IGetAllCandidatesWithSavedSearchesHandler, GetAllCandidatesWithSavedSearchesHandler>();
        services.AddTransient<IGetGetCandidateSavedSearchHandler, GetGetCandidateSavedSearchHandler>();
        services.AddTransient<IGetDormantCandidateAccountsHandler, GetDormantCandidateAccountsHandler>();
        services.AddTransient<ISendSavedSearchesNotificationHandler, SendSavedSearchesNotificationHandler>();
        services.AddTransient<IUpdateCandidateStatusHandler, UpdateCandidateStatusHandler>();
        services.AddTransient<IDateTimeService, DateTimeService>();
        services.AddTransient<IBatchTaskRunner, BatchTaskRunner>();
        services.AddHttpClient<IOuterApiClient, OuterApiClient>
            (
                options => options.Timeout = TimeSpan.FromMinutes(30)
            )
            .SetHandlerLifetime(TimeSpan.FromMinutes(10))
            .AddPolicyHandler(_ =>
            {
                return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                        retryAttempt)));
            });
        
        
        services
            .AddApplicationInsightsTelemetryWorkerService()
            .ConfigureFunctionsApplicationInsights();
    })
    .Build();

host.Run();