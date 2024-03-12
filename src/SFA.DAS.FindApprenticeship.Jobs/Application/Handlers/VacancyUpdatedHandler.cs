using System;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Recruit.Vacancies.Client.Domain.Events;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
public class VacancyUpdatedHandler : IVacancyUpdatedHandler
{
    private readonly IAzureSearchHelper _azureSearchHelperService;
    private readonly IRecruitService _recruitService;

    public VacancyUpdatedHandler(IAzureSearchHelper azureSearchHelper, IRecruitService recruitService)
    {
        _azureSearchHelperService = azureSearchHelper;
        _recruitService = recruitService;
    }

    public async Task Handle(LiveVacancyUpdatedEvent vacancyUpdatedEvent, ILogger log)
    {
        log.LogInformation($"Vacancy Updated Event handler invoked at {DateTime.UtcNow}");

        var alias = await _azureSearchHelperService.GetAlias(Domain.Constants.AliasName);
        var indexName = alias == null ? string.Empty : alias.Indexes.FirstOrDefault();

        if (string.IsNullOrWhiteSpace(indexName))
        {
            log.LogWarning($"Unable to update vacancy reference {vacancyUpdatedEvent.VacancyReference} - no index is aliased");
            return;
        }

        var vacancyReference = $"{vacancyUpdatedEvent.VacancyReference}";
        var document = await _azureSearchHelperService.GetDocument(indexName, vacancyReference);
        var updatedVacancy = await _recruitService.GetLiveVacancy(vacancyUpdatedEvent.VacancyReference.ToString());

        if (updatedVacancy == null)
        {
            log.LogInformation($"Unable to update vacancy reference {vacancyUpdatedEvent.VacancyReference} - vacancy not found");
            return;
        }

        document.Value.ClosingDate = updatedVacancy.ClosingDate;
        document.Value.StartDate = updatedVacancy.StartDate;

        var uploadBatch = Enumerable.Empty<ApprenticeAzureSearchDocument>().Append(document);
        await _azureSearchHelperService.UploadDocuments(indexName, uploadBatch);
    }
}
