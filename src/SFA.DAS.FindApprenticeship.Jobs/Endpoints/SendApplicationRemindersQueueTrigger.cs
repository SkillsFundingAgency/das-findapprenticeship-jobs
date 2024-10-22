using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints;

public class SendApplicationRemindersQueueTrigger(ISendApplicationReminderHandler handler)
{
    [Function("SendApplicationRemindersQueueTrigger")]
    public async Task Run([QueueTrigger(StorageQueueNames.VacancyClosing)] VacancyQueueItem vacancyQueueItem, ILogger log)
    {
        await handler.Handle(vacancyQueueItem.VacancyReference, vacancyQueueItem.DaysToExpire);
    }
}