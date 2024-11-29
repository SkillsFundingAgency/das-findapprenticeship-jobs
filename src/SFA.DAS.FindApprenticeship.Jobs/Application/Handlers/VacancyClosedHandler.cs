using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;

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
            await azureSearchHelper.DeleteDocument(indexName, $"{vacancyClosedEvent.VacancyReference}");
        }
        else
        {
            log.LogInformation($"Index {indexName} not found so document VAC{vacancyClosedEvent.VacancyReference} has not been deleted");
        }

        await findApprenticeshipJobsService.CloseVacancyEarly(vacancyClosedEvent.VacancyReference);
    }
}
