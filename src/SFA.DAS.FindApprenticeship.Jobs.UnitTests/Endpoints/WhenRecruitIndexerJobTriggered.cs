using AutoFixture.NUnit3;
using Microsoft.Azure.Functions.Worker;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Endpoints;
using SFA.DAS.Testing.AutoFixture;

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

        handler.Verify(x => x.Handle(), Times.Once());
    }
    
    [Test]
    [MoqInlineAutoData(5)]
    [MoqInlineAutoData(21)]
    public async Task Then_The_Index_Is_Not_Created_When_Outside_Of_Time_Frame(
        int hour,
        [Frozen] Mock<IRecruitIndexerJobHandler> handler,
        [Frozen] Mock<IDateTimeService> dateTimeService,
        RecruitIndexerTimerTrigger sut)
    {
        dateTimeService.Setup(x=>x.GetCurrentDateTime()).Returns(new DateTime(2020, 01, 01, hour,0,0));
        
        await sut.Run(It.IsAny<TimerInfo>());

        handler.Verify(x => x.Handle(), Times.Never());
    }
}
