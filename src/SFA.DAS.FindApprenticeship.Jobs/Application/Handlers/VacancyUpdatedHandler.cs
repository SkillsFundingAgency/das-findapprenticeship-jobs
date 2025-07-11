﻿using Esfa.Recruit.Vacancies.Client.Domain.Events;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
public class VacancyUpdatedHandler(
    IAzureSearchHelper azureSearchHelper,
    IFindApprenticeshipJobsService findApprenticeshipJobsService,
    ILogger<VacancyUpdatedHandler> log)
    : IVacancyUpdatedHandler
{

    public async Task Handle(LiveVacancyUpdatedEvent vacancyUpdatedEvent)
    {
        var vacancyReferenceIds = new List<string>();

        log.LogInformation($"Vacancy Updated Event handler invoked at {DateTime.UtcNow}");

        var alias = await azureSearchHelper.GetAlias(Domain.Constants.AliasName);
        var indexName = alias?.Indexes?.FirstOrDefault();

        if (string.IsNullOrWhiteSpace(indexName))
        {
            log.LogWarning($"Unable to update vacancy reference {vacancyUpdatedEvent.VacancyReference} - no index is aliased");
            return;
        }

        var updatedVacancy = await findApprenticeshipJobsService.GetLiveVacancy(vacancyUpdatedEvent.VacancyReference);
        vacancyReferenceIds.Add(updatedVacancy.Id);
        // TODO - ADDRESSES!!
        if (updatedVacancy.OtherAddresses is {Count: > 0})
        {
            var counter = 2;
            foreach (var azureSearchDocumentKey in updatedVacancy.OtherAddresses.Select(_ => $"{updatedVacancy.Id}-{counter}"))
            {
                vacancyReferenceIds.Add(azureSearchDocumentKey);
                counter++;
            }
        }
        await UpdateAzureSearchDocuments(indexName, vacancyReferenceIds, updatedVacancy.StartDate, updatedVacancy.ClosingDate);
    }

    private async Task UpdateAzureSearchDocuments(
        string indexName,
        List<string> vacancyReferenceIds,
        DateTime startDate,
        DateTime closingDate)
    {
        var documents = new List<ApprenticeAzureSearchDocument>();
        foreach (var vacancyReferenceId in vacancyReferenceIds)
        {
            var document = await azureSearchHelper.GetDocument(indexName, vacancyReferenceId);
            if (document == null)
            {
                continue;
            }
            document.Value.ClosingDate = closingDate;
            document.Value.StartDate = startDate;
            documents.Add(document);
        }

        if (documents.Count > 0)
        {
            await azureSearchHelper.UploadDocuments(indexName, documents);    
        }
    }
}
