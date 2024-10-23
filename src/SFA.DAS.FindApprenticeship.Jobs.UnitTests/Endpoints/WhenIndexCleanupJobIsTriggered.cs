using AutoFixture.NUnit3;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
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
            IndexCleanupTimerTrigger sut)
        {
            await sut.Run(It.IsAny<TimerInfo>());

            handler.Verify(x => x.Handle(log), Times.Once());
        }
    }
}