using AutoFixture.NUnit3;
using Microsoft.Azure.Functions.Worker;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Endpoints;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Endpoints;
public class WhenRecruitIndexerJobTriggered
{
    [Test, MoqAutoData]
    public async Task Then_The_Index_Is_Created(
        [Frozen] Mock<IRecruitIndexerJobHandler> handler,
        RecruitIndexerTimerTrigger sut)
    {
        await sut.Run(It.IsAny<TimerInfo>());

        handler.Verify(x => x.Handle(), Times.Once());
    }
}
