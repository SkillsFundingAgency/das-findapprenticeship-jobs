using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;

namespace SFA.DAS.FindApprenticeship.Jobs.Endpoints;

public class GetVacanciesClosingSoonTimerTrigger
{
    private readonly IVacancyClosingSoonHandler _handler;

    public GetVacanciesClosingSoonTimerTrigger(IVacancyClosingSoonHandler handler)
    {
        _handler = handler;
    }
    
    [FunctionName("GetVacanciesClosingSoonTimerTrigger")]
    public async Task Run([TimerTrigger("0 8 * * *")] TimerInfo myTimer, ILogger log,[Queue(StorageQueueNames.VacancyClosing)]ICollector<VacancyQueueItem> outputQueue)
    {
        
        log.LogInformation($"Application reminder function executed at: {DateTime.UtcNow}");

        var returnList = new List<VacancyQueueItem>();
        var vacanciesExpiringInTwoDays = await _handler.Handle(2);
        returnList.AddRange(vacanciesExpiringInTwoDays.Select(c =>new VacancyQueueItem{VacancyReference = c, DaysToExpire = 2}));
        var vacanciesExpiringInFiveDays = await _handler.Handle(5);
        returnList.AddRange(vacanciesExpiringInFiveDays.Select(c =>new VacancyQueueItem{VacancyReference = c, DaysToExpire = 5}));
        
        foreach (var vacancyQueueItem in returnList)
        {
            outputQueue.Add(vacancyQueueItem);
        }
    }
}

public class GetVacanciesClosingSoonHttpTrigger
{
    private readonly IVacancyClosingSoonHandler _handler;

    public GetVacanciesClosingSoonHttpTrigger(IVacancyClosingSoonHandler handler)
    {
        _handler = handler;
    }
    
    [FunctionName("GetVacanciesClosingSoonHttpTrigger")]
    public async Task Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestMessage req, ILogger log,[Queue(StorageQueueNames.VacancyClosing)]ICollector<VacancyQueueItem> outputQueue)
    {
        log.LogInformation($"Application reminder function executed at: {DateTime.UtcNow}");

        var returnList = new List<VacancyQueueItem>();
        var vacanciesExpiringInTwoDays = await _handler.Handle(2);
        returnList.AddRange(vacanciesExpiringInTwoDays.Select(c =>new VacancyQueueItem{VacancyReference = c, DaysToExpire = 2}));
        var vacanciesExpiringInFiveDays = await _handler.Handle(5);
        returnList.AddRange(vacanciesExpiringInFiveDays.Select(c =>new VacancyQueueItem{VacancyReference = c, DaysToExpire = 5}));

        foreach (var vacancyQueueItem in returnList)
        {
            outputQueue.Add(vacancyQueueItem);
        }
    }
}


public class VacancyQueueItem
{
    public long VacancyReference { get; set; }
    public int DaysToExpire { get; set; }
}