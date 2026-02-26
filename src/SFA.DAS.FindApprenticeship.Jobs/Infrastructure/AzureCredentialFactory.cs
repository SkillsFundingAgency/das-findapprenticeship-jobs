using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Configuration;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure;

public static class AzureCredentialFactory
{
    public static TokenCredential BuildCredential(IConfiguration configuration)
    {
        const int maxRetries = 2;
        var networkTimeout = TimeSpan.FromMilliseconds(500);
        var delay = TimeSpan.FromMilliseconds(100);

        var resourceEnvironmentName = configuration["ResourceEnvironmentName"];
        var isLocal = resourceEnvironmentName != null &&
                      resourceEnvironmentName.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase);

        if (isLocal)
        {
            return new ChainedTokenCredential(
                new AzureCliCredential(new AzureCliCredentialOptions
                {
                    Retry = { NetworkTimeout = networkTimeout, MaxRetries = maxRetries, Delay = delay, Mode = RetryMode.Fixed }
                }),
                new VisualStudioCredential(new VisualStudioCredentialOptions
                {
                    Retry = { NetworkTimeout = networkTimeout, MaxRetries = maxRetries, Delay = delay, Mode = RetryMode.Fixed }
                }),
                new VisualStudioCodeCredential(new VisualStudioCodeCredentialOptions
                {
                    Retry = { NetworkTimeout = networkTimeout, MaxRetries = maxRetries, Delay = delay, Mode = RetryMode.Fixed }
                }));
        }

        return new ChainedTokenCredential(
            new ManagedIdentityCredential(new ManagedIdentityCredentialOptions
            {
                Retry = { NetworkTimeout = networkTimeout, MaxRetries = maxRetries, Delay = delay, Mode = RetryMode.Fixed }
            }),
            new AzureCliCredential(new AzureCliCredentialOptions
            {
                Retry = { NetworkTimeout = networkTimeout, MaxRetries = maxRetries, Delay = delay, Mode = RetryMode.Fixed }
            }),
            new VisualStudioCredential(new VisualStudioCredentialOptions
            {
                Retry = { NetworkTimeout = networkTimeout, MaxRetries = maxRetries, Delay = delay, Mode = RetryMode.Fixed }
            }),
            new VisualStudioCodeCredential(new VisualStudioCodeCredentialOptions
            {
                Retry = { NetworkTimeout = networkTimeout, MaxRetries = maxRetries, Delay = delay, Mode = RetryMode.Fixed }
            }));
    }
}
