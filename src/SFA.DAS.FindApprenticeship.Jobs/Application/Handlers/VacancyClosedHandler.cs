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
        var indexName = alias?.Indexes.FirstOrDefault();

        if (string.IsNullOrEmpty(indexName))
        {
            log.LogInformation("Index {IndexName} not found so document VAC{VacancyReference} has not been deleted", indexName, vacancyClosedEvent.VacancyReference);
            await findApprenticeshipJobsService.CloseVacancyEarly(vacancyClosedEvent.VacancyReference);
            return;
        }

        var ids = new List<string> { vacancyClosedEvent.VacancyReference.ToString() };
        var document = await azureSearchHelper.GetDocument(indexName, ids[0]);
        if (document != null)
        {
            if (document is { Value.OtherAddresses.Count: > 0})
            {
                // Note: multiple locations vacancy has ids such that: xxxx, xxxx-2, xxxx-3 etc. There is no '1' document.
                ids.AddRange(Enumerable.Range(1, document.Value.OtherAddresses.Count).Select(x => $"{ids[0]}-{x+1}"));
            }

            await azureSearchHelper.DeleteDocuments(indexName, ids);
        }
        
        await findApprenticeshipJobsService.CloseVacancyEarly(vacancyClosedEvent.VacancyReference);
    }
}