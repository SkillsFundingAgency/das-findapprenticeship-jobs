using Microsoft.AspNetCore.Components;
using Microsoft.Azure.WebJobs.Host.Config;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.DependencyInjection;
internal class InjectConfiguration : IExtensionConfigProvider
{
    public readonly InjectBindingProvider InjectBindingProvider;

    public InjectConfiguration(InjectBindingProvider injectBindingProvider) =>
        InjectBindingProvider = injectBindingProvider;

    public void Initialize(ExtensionConfigContext context) => context
        .AddBindingRule<InjectAttribute>()
        .Bind(InjectBindingProvider);
}
