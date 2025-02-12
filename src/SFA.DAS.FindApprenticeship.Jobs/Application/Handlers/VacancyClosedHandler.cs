﻿using Esfa.Recruit.Vacancies.Client.Domain.Events;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;

public class VacancyClosedHandler(
    IAzureSearchHelper azureSearchHelper,
    IFindApprenticeshipJobsService findApprenticeshipJobsService,
    ILogger<VacancyClosedHandler> log)
    : IVacancyClosedHandler
{
    public async Task Handle(VacancyClosedEvent vacancyClosedEvent)
    {
        var vacancyReferenceId = $"{vacancyClosedEvent.VacancyReference}";
        var alias = await azureSearchHelper.GetAlias(Domain.Constants.AliasName);
        var indexName = alias?.Indexes.FirstOrDefault();

        if (string.IsNullOrEmpty(indexName))
        {
            log.LogInformation("Index {IndexName} not found so document VAC{VacancyReference} has not been deleted", indexName, vacancyClosedEvent.VacancyReference);
            await findApprenticeshipJobsService.CloseVacancyEarly(vacancyClosedEvent.VacancyReference);
            return;
        }

        var ids = new List<string> { vacancyClosedEvent.VacancyReference.ToString() };
        var document = await azureSearchHelper.GetDocument(indexName, ids[0]);
        if (document is { Value.OtherAddresses.Count: > 0})
        {
            ids.AddRange(Enumerable.Range(1, document.Value.OtherAddresses.Count).Select(x => $"{ids[0]}-{x}"));
        }

        await azureSearchHelper.DeleteDocuments(indexName, ids);
        await findApprenticeshipJobsService.CloseVacancyEarly(vacancyClosedEvent.VacancyReference);
    }
}