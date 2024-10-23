using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints;

public class GetVacanciesClosingSoonTimerTrigger(IVacancyClosingSoonHandler handler, ILogger<GetVacanciesClosingSoonTimerTrigger> log)
{
    [QueueOutput(StorageQueueNames.VacancyClosing)]
    [Function("GetVacanciesClosingSoonTimerTrigger")]
    public async Task<List<VacancyQueueItem>> Run([TimerTrigger("0 8 * * *")] TimerInfo myTimer)
    {
        log.LogInformation($"Application reminder function executed at: {DateTime.UtcNow}");

        var returnList = new List<VacancyQueueItem>();
        var vacanciesExpiringInTwoDays = await handler.Handle(2);
        returnList.AddRange(vacanciesExpiringInTwoDays.Select(c =>new VacancyQueueItem{VacancyReference = c, DaysToExpire = 2}));
        var vacanciesExpiringInFiveDays = await handler.Handle(7);
        returnList.AddRange(vacanciesExpiringInFiveDays.Select(c =>new VacancyQueueItem{VacancyReference = c, DaysToExpire = 7}));

        return returnList.ToList();
    }
}

public class GetVacanciesClosingSoonHttpTrigger(IVacancyClosingSoonHandler handler, ILogger<GetVacanciesClosingSoonHttpTrigger> log)
{
    [QueueOutput(StorageQueueNames.VacancyClosing)]
    [Function("GetVacanciesClosingSoonHttpTrigger")]
    public async Task<List<VacancyQueueItem>> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestMessage req)
    {
        log.LogInformation($"Application reminder function executed at: {DateTime.UtcNow}");

        var returnList = new List<VacancyQueueItem>();
        var vacanciesExpiringInTwoDays = await handler.Handle(2);
        returnList.AddRange(vacanciesExpiringInTwoDays.Select(c =>new VacancyQueueItem{VacancyReference = c, DaysToExpire = 2}));
        var vacanciesExpiringInFiveDays = await handler.Handle(7);
        returnList.AddRange(vacanciesExpiringInFiveDays.Select(c =>new VacancyQueueItem{VacancyReference = c, DaysToExpire = 7}));

        return returnList;
    }
}


public class VacancyQueueItem
{
    public long VacancyReference { get; set; }
    public int DaysToExpire { get; set; }
}