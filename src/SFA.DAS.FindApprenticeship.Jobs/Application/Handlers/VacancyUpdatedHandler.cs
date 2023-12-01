using System;
using System.Linq;
using System.Threading.Tasks;
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
    private readonly IDateTimeService _dateTimeService;
    public VacancyUpdatedHandler(IAzureSearchHelper azureSearchHelper, IRecruitService recruitService, IDateTimeService dateTimeService)
    {
        _azureSearchHelperService = azureSearchHelper;
        _recruitService = recruitService;
        _dateTimeService = dateTimeService;
    }

    public async Task Handle(VacancyUpdatedEvent vacancyUpdatedEvent, ILogger log)
    {
        log.LogInformation($"Vacancy Updated Event handler invoked at {DateTime.UtcNow}");

        //var alias = await _azureSearchHelperService.GetAlias(Domain.Constants.AliasName);
        //var indexName = alias == null ? string.Empty : alias.Indexes.FirstOrDefault();

        //var vacancyReference = $"VAC{vacancyUpdatedEvent.VacancyReference}";
        //var document = await _azureSearchHelperService.GetDocument(indexName, vacancyReference);
        //var updatedVacancy = await _recruitService.GetLiveVacancy(vacancyUpdatedEvent.VacancyReference);

        //if (vacancyUpdatedEvent.UpdateKind.HasFlag(LiveUpdateKind.ClosingDate))
        //{
        //    document.Value.ClosingDate = updatedVacancy.LiveVacancy.ClosingDate;
        //}
        //if (vacancyUpdatedEvent.UpdateKind.HasFlag(LiveUpdateKind.StartDate))
        //{
        //    document.Value.StartDate = updatedVacancy.LiveVacancy.StartDate;
        //}

        //var uploadBatch = Enumerable.Empty<ApprenticeAzureSearchDocument>().Append(document);
        //await _azureSearchHelperService.UploadDocuments(indexName, uploadBatch);
    }
}
