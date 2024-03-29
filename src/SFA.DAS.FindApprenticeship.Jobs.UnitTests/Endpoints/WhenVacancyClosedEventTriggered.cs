﻿using AutoFixture.NUnit3;
using Esfa.Recruit.Vacancies.Client.Domain.Events;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Endpoints;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Endpoints;
public class WhenVacancyClosedEventTriggered
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Will_Be_Handled(
    ILogger log,
    VacancyClosedEvent command,
    [Frozen] Mock<IVacancyClosedHandler> handler,
    HandleVacancyClosedEvent sut)
    {
        await sut.Run(command, log);

        handler.Verify(x => x.Handle(It.Is<VacancyClosedEvent>(x => x.VacancyId == command.VacancyId), log), Times.Once());
    }
}
