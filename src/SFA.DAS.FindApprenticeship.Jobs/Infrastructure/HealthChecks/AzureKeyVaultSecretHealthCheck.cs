using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;
using System.Security.Cryptography.X509Certificates;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.HealthChecks;

public class AzureKeyVaultSecretHealthCheck : IHealthCheck
{
    private readonly FindApprenticeshipJobsConfiguration _config;

    public AzureKeyVaultSecretHealthCheck(IOptions<FindApprenticeshipJobsConfiguration> config)
    {
        _config = config.Value;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrEmpty(_config.SecretClientUrl) || string.IsNullOrEmpty(_config.SecretName))
            {
                // Config missing → degraded, not healthy
                return HealthCheckResult.Degraded("Key Vault secret configuration is missing.");
            }

            var credential = new DefaultAzureCredential();
            var secretClient = new SecretClient(new Uri(_config.SecretClientUrl!), credential);

            var secret = await secretClient.GetSecretAsync(
                _config.SecretName!,
                cancellationToken: cancellationToken);

            if (!secret.HasValue)
            {
                return HealthCheckResult.Degraded(
                    $"Key Vault secret has no value. Raw response: {secret.GetRawResponse().Content.ToDynamicFromJson()}");
            }

            // Optional: confirm it’s a valid certificate payload
            _ = new X509Certificate2(Convert.FromBase64String(secret.Value.Value));

            return HealthCheckResult.Healthy("Key Vault Secret Client is healthy");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Degraded("Key Vault Secret Client check failed", ex);
        }
    }
}
