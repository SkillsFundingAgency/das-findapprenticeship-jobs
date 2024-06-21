using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints;

public class SendApplicationRemindersQueueTrigger
{
    private readonly ISendApplicationReminderHandler _handler;

    public SendApplicationRemindersQueueTrigger(ISendApplicationReminderHandler handler)
    {
        _handler = handler;
    }
    
    [FunctionName("SendApplicationRemindersQueueTrigger")]
    public async Task Run([QueueTrigger(StorageQueueNames.VacancyClosing)] VacancyQueueItem vacancyQueueItem, ILogger log)
    {
        await _handler.Handle(vacancyQueueItem.VacancyReference, vacancyQueueItem.DaysToExpire);
    }
}