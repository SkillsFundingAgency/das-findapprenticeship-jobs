using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;

namespace SFA.DAS.FindApprenticeship.Jobs.AcceptanceTests.Infrastructure;
public static class ConfigBuilder
{
    public static IConfigurationRoot GenerateConfiguration()
    {
        var configSource = new MemoryConfigurationSource
        {
            InitialData = new[]
    {
                new KeyValuePair<string, string>("ConfigurationStorageConnectionString", "UseDevelopmentStorage=true;"),
                new KeyValuePair<string, string>("ConfigNames", "SFA.DAS.FindApprenticeship.Jobs,SFA.DAS.Encoding"),
                new KeyValuePair<string, string>("EnvironmentName", "DEV"),
                new KeyValuePair<string, string>("Version", "1.0"),
                new KeyValuePair<string, string>("ApimBaseUrl", "http://localhost:1234"),
                new KeyValuePair<string, string>("ApimKey", "test"),
                new KeyValuePair<string, string>("AzureSearchBaseUrl", "http://localhost:5678"),
                new KeyValuePair<string, string>("AzureSearchResource", "http://localhost:9101"),
                new KeyValuePair<string, string>("AzureSearchKey", "test")
            }
        };

        var provider = new MemoryConfigurationProvider(configSource);
        return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
    }
}
