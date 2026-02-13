using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Endpoints;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Endpoints;

public class WhenIndexCleanupTimerIsTriggered
{
    [Test, MoqAutoData]
    public async Task Then_Index_Cleanup_Is_Invoked(
        ILogger log,
        [Frozen] Mock<IIndexCleanupJobHandler> handler,
        [Frozen] Mock<IDateTimeService> dateTimeService,
        IndexCleanupTimerTrigger sut)
    {
        dateTimeService.Setup(x=>x.GetCurrentDateTime()).Returns(new DateTime(2020, 01, 01, 07,0,0));
            
        await sut.Run(It.IsAny<TimerInfo>());

        handler.Verify(x => x.Handle(), Times.Once());
    }
}