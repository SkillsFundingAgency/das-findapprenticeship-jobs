using NServiceBus;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;
using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.NServiceBus.Configuration.AzureServiceBus;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;

namespace SFA.DAS.FindApprenticeship.Jobs.TestHarness
{
    internal static class Program
    {
        private const string AzureServiceBusConnectionString = "";

        public static async Task Main()
        {
            var endpointConfiguration = new EndpointConfiguration(QueueNames.TestHarness)
                .UseErrorQueue(QueueNames.VacancyUpdated + "-error")
                .UseInstallers()
                .UseMessageConventions()
                .UseNewtonsoftJsonSerializer()
            ;

            if (!string.IsNullOrEmpty(AzureServiceBusConnectionString))
            {
                endpointConfiguration
                    .UseAzureServiceBusTransport(AzureServiceBusConnectionString, _ => { });
            }
            else
            {
                endpointConfiguration
                    .UseTransport<LearningTransport>()
                    .Transactions(TransportTransactionMode.ReceiveOnly)
                    .StorageDirectory(Path.Combine(Path.GetTempPath(),
                    ".learningtransport"));
            }

            var endpoint = await Endpoint.Start(endpointConfiguration);

            Console.WriteLine("*** SFA.DAS.NServiceBus.TestEndpoint ***");
            Console.WriteLine("Press '1' to publish event");
            Console.WriteLine("Press any other key to exit");

            while (true)
            {
                var key = Console.ReadKey();

                Console.WriteLine();

                if (key.Key == ConsoleKey.D1)
                {
                    var vacancyId = Guid.NewGuid();
                    await endpoint.Publish(new LiveVacancyUpdatedEvent() { VacancyId = vacancyId, VacancyReference = 36, UpdateKind = LiveUpdateKind.StartDate });
                }
                else
                {
                    break;
                }
            }

            await endpoint.Stop();
        }
    }
}