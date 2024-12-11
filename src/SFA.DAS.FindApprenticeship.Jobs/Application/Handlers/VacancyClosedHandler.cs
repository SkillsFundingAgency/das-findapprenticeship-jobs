using Esfa.Recruit.Vacancies.Client.Domain.Events;
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
        var alias = await azureSearchHelper.GetAlias(Domain.Constants.AliasName);
        var indexName = alias == null ? string.Empty : alias.Indexes.FirstOrDefault();

        if (!string.IsNullOrEmpty(indexName))
        {
            var vacancyIds = new List<string>
            {
                $"{vacancyClosedEvent.VacancyReference}"
            };

            var vacancy = await findApprenticeshipJobsService.GetLiveVacancy(vacancyClosedEvent.VacancyReference.ToString());

            if (vacancy.OtherAddresses is { Count: > 0 })
            {
                var counter = 1;
                foreach (var azureSearchDocumentKey in vacancy.OtherAddresses.Select(_ => $"{vacancy.Id}-{counter}"))
                {
                    vacancyIds.Add(azureSearchDocumentKey);
                    counter++;
                }
            }
            await azureSearchHelper.DeleteDocuments(indexName, vacancyIds);
        }
        else
        {
            log.LogInformation($"Index {indexName} not found so document VAC{vacancyClosedEvent.VacancyReference} has not been deleted");
        }

        await findApprenticeshipJobsService.CloseVacancyEarly(vacancyClosedEvent.VacancyReference);
    }
}
