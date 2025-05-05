using AutoFixture.NUnit3;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Endpoints;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Endpoints
{
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
        
        [Test]
        [MoqInlineAutoData(5)]
        [MoqInlineAutoData(21)]
        public async Task Then_Index_Cleanup_Is_Not_Invoked_When_Outside_Of_Period(
            int hour,
            ILogger log,
            [Frozen] Mock<IIndexCleanupJobHandler> handler,
            [Frozen] Mock<IDateTimeService> dateTimeService,
            IndexCleanupTimerTrigger sut)
        {
            dateTimeService.Setup(x=>x.GetCurrentDateTime()).Returns(new DateTime(2020, 01, 01, hour,0,0));
            
            await sut.Run(It.IsAny<TimerInfo>());

            handler.Verify(x => x.Handle(), Times.Never());
        }
    }
}