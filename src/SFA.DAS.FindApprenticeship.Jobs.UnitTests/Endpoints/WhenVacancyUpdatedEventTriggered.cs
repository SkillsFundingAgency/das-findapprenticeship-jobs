using AutoFixture.NUnit3;
using Esfa.Recruit.Vacancies.Client.Domain.Events;
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
        LiveVacancyUpdatedEvent message,
        ILogger log,
        [Frozen] Mock<IVacancyUpdatedHandler> handler,
        HandleVacancyUpdatedEvent sut)
    {
        await sut.Run(message, log);

        handler.Verify(x => x.Handle(It.Is<LiveVacancyUpdatedEvent>(x => x.VacancyId == message.VacancyId), log), Times.Once());
    }
}
