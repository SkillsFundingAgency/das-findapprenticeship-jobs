using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
public class VacancyDeletedHandler : IVacancyDeletedHandler
{
    private readonly IAzureSearchHelper _azureSearchHelperService;
    private readonly ILogger<VacancyUpdatedHandler> _logger;
    public VacancyDeletedHandler(IAzureSearchHelper azureSearchHelper, ILogger<VacancyUpdatedHandler> logger)
    {
        _azureSearchHelperService = azureSearchHelper;
        _logger = logger;
    }
    public async Task Handle(VacancyDeletedEvent vacancyDeletedEvent)
    {
        var alias = await _azureSearchHelperService.GetAlias(Domain.Constants.AliasName);
        var indexName = alias == null ? string.Empty : alias.Indexes.FirstOrDefault();

        if (!string.IsNullOrEmpty(indexName))
        {
            await _azureSearchHelperService.DeleteDocument(indexName, $"VAC{vacancyDeletedEvent.VacancyId}");
        }
        else
        {
            _logger.LogInformation($"Index {indexName} not found so document VAC{vacancyDeletedEvent.VacancyId} has not been deleted");
        }
    }
}
