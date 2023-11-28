using AutoFixture.NUnit3;
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
        ILogger<VacancyUpdatedEvent> logger,
        VacancyUpdatedEvent message,
        [Frozen] Mock<IVacancyUpdatedHandler> handler,
        HandleVacancyUpdatedEvent sut)
    {
        await sut.Run(message, logger);

        handler.Verify(x => x.Handle(It.Is<VacancyUpdatedEvent>(x => x.VacancyId == message.VacancyId)), Times.Once());
    }
}
