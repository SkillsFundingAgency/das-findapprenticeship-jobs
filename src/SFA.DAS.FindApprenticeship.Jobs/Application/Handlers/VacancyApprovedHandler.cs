using Esfa.Recruit.Vacancies.Client.Domain.Events;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;

public class VacancyApprovedHandler(
    IAzureSearchHelper azureSearchHelper,
    IFindApprenticeshipJobsService findApprenticeshipJobsService,
    ILogger<VacancyApprovedHandler> log,
    IApprenticeAzureSearchDocumentFactory documentFactory)
    : IVacancyApprovedHandler
{
    public async Task Handle(VacancyApprovedEvent vacancyApprovedEvent)
    {
        log.LogInformation("Vacancy Approved Event handler invoked at {DateTime}", DateTime.UtcNow);

        var alias = await azureSearchHelper.GetAlias(Domain.Constants.AliasName);
        var indexName = alias?.Indexes.FirstOrDefault();

        var approvedVacancy = await findApprenticeshipJobsService.GetLiveVacancy(vacancyApprovedEvent.VacancyReference);
        if (!string.IsNullOrEmpty(indexName))
        {
            var documents = documentFactory.Create(approvedVacancy);
            await azureSearchHelper.UploadDocuments(indexName, documents);
        }
        else
        {
            log.LogInformation("Handle VacancyApprovedEvent failed with indexName {IndexName} and vacancyId {VacancyId}", indexName, vacancyApprovedEvent.VacancyId);
        }
    }
}