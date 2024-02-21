using System.Linq;
using System.Threading.Tasks;
using Esfa.Recruit.Vacancies.Client.Domain.Events;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
public class VacancyClosedHandler : IVacancyClosedHandler
{
    private readonly IAzureSearchHelper _azureSearchHelperService;
    public VacancyClosedHandler(IAzureSearchHelper azureSearchHelper)
    {
        _azureSearchHelperService = azureSearchHelper;
    }
    public async Task Handle(VacancyClosedEvent vacancyClosedEvent, ILogger log)
    {
        var alias = await _azureSearchHelperService.GetAlias(Domain.Constants.AliasName);
        var indexName = alias == null ? string.Empty : alias.Indexes.FirstOrDefault();

        if (!string.IsNullOrEmpty(indexName))
        {
            await _azureSearchHelperService.DeleteDocument(indexName, $"{vacancyClosedEvent.VacancyReference}");
        }
        else
        {
            log.LogInformation($"Index {indexName} not found so document VAC{vacancyClosedEvent.VacancyReference} has not been deleted");
        }
    }
}
