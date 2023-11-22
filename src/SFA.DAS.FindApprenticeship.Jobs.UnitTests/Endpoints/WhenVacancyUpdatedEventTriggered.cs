using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Endpoints;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Endpoints;
public class WhenVacancyUpdatedEventTriggered
{
    [Test, MoqAutoData]
    public async Task Then_The_Message_Will_Be_Handled(
        VacancyUpdatedEvent message,
        Mock<IVacancyUpdatedHandler> handler)
    {
        await HandleVacancyUpdatedEvent.Run(message, handler.Object, It.IsAny<ILogger<VacancyUpdatedEvent>>());

        handler.Verify(x => x.Handle(It.IsAny<VacancyUpdatedEvent>()), Times.Once());
    }
}
