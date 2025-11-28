using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Endpoints;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Endpoints;

public class WhenRecruitIndexerJobTriggered
{
    [Test, MoqAutoData]
    public async Task Then_The_Index_Is_Created(
        [Frozen] Mock<IRecruitIndexerJobHandler> handler,
        [Frozen] Mock<IDateTimeService> dateTimeService,
        RecruitIndexerTimerTrigger sut)
    {
        dateTimeService.Setup(x=>x.GetCurrentDateTime()).Returns(new DateTime(2020, 01, 01, 07,0,0));
        
        await sut.Run(It.IsAny<TimerInfo>());

        handler.Verify(x => x.Handle(CancellationToken.None), Times.Once());
    }
}
