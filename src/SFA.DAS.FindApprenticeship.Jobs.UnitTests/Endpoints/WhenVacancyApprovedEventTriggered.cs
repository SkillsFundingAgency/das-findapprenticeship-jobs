﻿using AutoFixture.NUnit3;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Endpoints;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Endpoints;
public class WhenVacancyApprovedEventTriggered
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Will_Be_Handled(
        ILogger log,
        VacancyApprovedEvent command,
        [Frozen] Mock<IVacancyApprovedHandler> handler,
        HandleVacancyApprovedEvent sut)
    {
        await sut.Run(command, log);

        handler.Verify(x => x.Handle(It.Is<VacancyApprovedEvent>(x => x.VacancyId == command.VacancyId), log), Times.Once());
    }
}