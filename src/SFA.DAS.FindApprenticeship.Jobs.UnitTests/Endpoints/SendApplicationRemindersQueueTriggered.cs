using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Endpoints;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Endpoints;

public class SendApplicationRemindersQueueTriggered
{
    [Test, MoqAutoData]
    public async Task Then_The_Queue_Item_Is_Processed_And_SendReminderHandler_Called(
        ILogger log,
        VacancyQueueItem queueItem,
        [Frozen] Mock<ISendApplicationReminderHandler> handler,
        SendApplicationRemindersQueueTrigger trigger)
    {
        await trigger.Run(queueItem);
        
        handler.Verify(x=>x.Handle(queueItem.VacancyReference, queueItem.DaysToExpire), Times.Once);
    }
}