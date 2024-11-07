using Esfa.Recruit.Vacancies.Client.Domain.Events;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
public class VacancyApprovedHandler(
    IAzureSearchHelper azureSearchHelper,
    IRecruitService recruitService,
    ILogger<VacancyApprovedHandler> log)
    : IVacancyApprovedHandler
{
    public async Task Handle(VacancyApprovedEvent vacancyApprovedEvent)
    {
        log.LogInformation($"Vacancy Approved Event handler invoked at {DateTime.UtcNow}");

        var approvedVacancy = await recruitService.GetLiveVacancy(vacancyApprovedEvent.VacancyReference.ToString());

        var alias = await azureSearchHelper.GetAlias(Domain.Constants.AliasName);
        var indexName = alias == null ? string.Empty : alias.Indexes.FirstOrDefault();

        if (approvedVacancy != null && !string.IsNullOrEmpty(indexName))
        {
            var document = (ApprenticeAzureSearchDocument)approvedVacancy;
            await azureSearchHelper.UploadDocuments(indexName, Enumerable.Empty<ApprenticeAzureSearchDocument>().Append(document));
        }
        else
        {
            log.LogInformation($"Handle VacancyApprovedEvent failed with indexName {indexName} and vacancyId {vacancyApprovedEvent.VacancyId}");
        }
    }

}
