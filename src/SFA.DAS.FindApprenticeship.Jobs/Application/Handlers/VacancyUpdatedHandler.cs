using Esfa.Recruit.Vacancies.Client.Domain.Events;
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
        log.LogInformation($"Vacancy Updated Event handler invoked at {DateTime.UtcNow}");

        var alias = await azureSearchHelper.GetAlias(Domain.Constants.AliasName);
        var indexName = alias == null ? string.Empty : alias.Indexes.FirstOrDefault();

        if (string.IsNullOrWhiteSpace(indexName))
        {
            log.LogWarning($"Unable to update vacancy reference {vacancyUpdatedEvent.VacancyReference} - no index is aliased");
            return;
        }

        var updatedVacancy = await findApprenticeshipJobsService.GetLiveVacancy(vacancyUpdatedEvent.VacancyReference.ToString());
        
        if (updatedVacancy.OtherAddresses is {Count: > 0})
        {
            var counter = 1;
            foreach (var azureSearchDocumentKey in updatedVacancy.OtherAddresses.Select(_ => $"{updatedVacancy.Id}-{counter}"))
            {
                await UpdateAzureSearchDocument(indexName, azureSearchDocumentKey, updatedVacancy.StartDate, updatedVacancy.ClosingDate);
                counter++;
            }
        }

        await UpdateAzureSearchDocument(indexName, updatedVacancy.Id, updatedVacancy.StartDate, updatedVacancy.ClosingDate);
    }

    private async Task UpdateAzureSearchDocument(
        string indexName,
        string vacancyReference,
        DateTime startDate,
        DateTime closingDate)
    {
        var document = await azureSearchHelper.GetDocument(indexName, vacancyReference);

        document.Value.ClosingDate = closingDate;
        document.Value.StartDate = startDate;

        var uploadBatch = Enumerable.Empty<ApprenticeAzureSearchDocument>().Append(document);
        await azureSearchHelper.UploadDocuments(indexName, uploadBatch);
    }
}
