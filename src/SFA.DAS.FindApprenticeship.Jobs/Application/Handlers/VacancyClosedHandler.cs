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
        var vacancyReferenceId = $"{vacancyClosedEvent.VacancyReference}";
        var alias = await azureSearchHelper.GetAlias(Domain.Constants.AliasName);
        var indexName = alias?.Indexes?.FirstOrDefault();

        if (!string.IsNullOrEmpty(indexName))
        {
            var vacancyIds = new List<string>
            {
                vacancyReferenceId
            };

            var document = await azureSearchHelper.GetDocument(indexName, $"{vacancyReferenceId}");

            if (document is {Value.OtherAddresses.Count: > 0})
            {
                var counter = 1;
                foreach (var azureSearchDocumentKey in document.Value.OtherAddresses.Select(_ =>
                             $"{document.Value.Id}-{counter}"))
                {
                    vacancyIds.Add(azureSearchDocumentKey);
                    counter++;
                }
            }
            await azureSearchHelper.DeleteDocuments(indexName, vacancyIds);
        }
        else
        {
            log.LogInformation($"Index {indexName} not found so document VAC{vacancyReferenceId} has not been deleted");
        }

        await findApprenticeshipJobsService.CloseVacancyEarly(vacancyClosedEvent.VacancyReference);
    }
}