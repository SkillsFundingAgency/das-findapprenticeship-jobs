using System.Linq;
using System.Threading.Tasks;
using Esfa.Recruit.Vacancies.Client.Domain.Events;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
public class VacancyDeletedHandler : IVacancyDeletedHandler
{
    private readonly IAzureSearchHelper _azureSearchHelperService;
    public VacancyDeletedHandler(IAzureSearchHelper azureSearchHelper)
    {
        _azureSearchHelperService = azureSearchHelper;
    }
    public async Task Handle(VacancyDeletedEvent vacancyDeletedEvent, ILogger log)
    {
        var alias = await _azureSearchHelperService.GetAlias(Domain.Constants.AliasName);
        var indexName = alias == null ? string.Empty : alias.Indexes.FirstOrDefault();

        if (!string.IsNullOrEmpty(indexName))
        {
            await _azureSearchHelperService.DeleteDocument(indexName, $"{vacancyDeletedEvent.VacancyId}");
        }
        else
        {
            log.LogInformation($"Index {indexName} not found so document VAC{vacancyDeletedEvent.VacancyId} has not been deleted");
        }
    }
}
