using Esfa.Recruit.Vacancies.Client.Domain.Events;
using SFA.DAS.FindApprenticeship.Jobs.Application.Extensions;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
public class VacancyApprovedHandler(
    IAzureSearchHelper azureSearchHelper,
    IFindApprenticeshipJobsService findApprenticeshipJobsService,
    ILogger<VacancyApprovedHandler> log)
    : IVacancyApprovedHandler
{
    public async Task Handle(VacancyApprovedEvent vacancyApprovedEvent)
    {
        log.LogInformation($"Vacancy Approved Event handler invoked at {DateTime.UtcNow}");

        var approvedVacancy = await findApprenticeshipJobsService.GetLiveVacancy(vacancyApprovedEvent.VacancyReference.ToString());

        var alias = await azureSearchHelper.GetAlias(Domain.Constants.AliasName);
        var indexName = alias == null ? string.Empty : alias.Indexes.FirstOrDefault();

        if (!string.IsNullOrEmpty(indexName))
        {
            if (approvedVacancy is { OtherAddresses.Count: > 0 })
            {
                var vacanciesWithMultipleLocations = LiveVacancyExtensions.SplitLiveVacanciesToMultipleByLocation(new List<LiveVacancy> { approvedVacancy });
                await azureSearchHelper.UploadDocuments(indexName, vacanciesWithMultipleLocations.Select(doc => (ApprenticeAzureSearchDocument)doc));
            }
            else
            {
                var document = (ApprenticeAzureSearchDocument)approvedVacancy;
                await azureSearchHelper.UploadDocuments(indexName, Enumerable.Empty<ApprenticeAzureSearchDocument>().Append(document));
            }
        }
        else
        {
            log.LogInformation($"Handle VacancyApprovedEvent failed with indexName {indexName} and vacancyId {vacancyApprovedEvent.VacancyId}");
        }
    }
}