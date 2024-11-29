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

        var vacancyReference = $"{vacancyUpdatedEvent.VacancyReference}";
        var document = await azureSearchHelper.GetDocument(indexName, vacancyReference);
        var updatedVacancy = await findApprenticeshipJobsService.GetLiveVacancy(vacancyUpdatedEvent.VacancyReference.ToString());

        if (updatedVacancy == null)
        {
            log.LogInformation($"Unable to update vacancy reference {vacancyUpdatedEvent.VacancyReference} - vacancy not found");
            return;
        }

        document.Value.ClosingDate = updatedVacancy.ClosingDate;
        document.Value.StartDate = updatedVacancy.StartDate;

        var uploadBatch = Enumerable.Empty<ApprenticeAzureSearchDocument>().Append(document);
        await azureSearchHelper.UploadDocuments(indexName, uploadBatch);
    }
}
