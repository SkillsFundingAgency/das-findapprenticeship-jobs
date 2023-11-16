using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using System;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.DependencyInjection;
public static class DependencyInjectionWebJobsBuilderExtensions
{
    public static IWebJobsBuilder AddDependencyInjection<TServiceProviderBuilder>(this IWebJobsBuilder builder)
        where TServiceProviderBuilder : IServiceProviderBuilder
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Services.AddSingleton(typeof(IServiceProviderBuilder), typeof(TServiceProviderBuilder));
        AddCommonDependencyInjection(builder);

        return builder;
    }

    private static void AddCommonDependencyInjection(IWebJobsBuilder builder)
    {
        builder.Services.AddSingleton(provider =>
        {
            var serviceProviderBuilder = provider.GetRequiredService<IServiceProviderBuilder>();
            return new ServiceProviderHolder(serviceProviderBuilder.Build());
        });

        builder.Services.AddSingleton<InjectBindingProvider>();
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IFunctionFilter, ScopeCleanupFilter>());
        builder.AddExtension<InjectConfiguration>();
    }
}
