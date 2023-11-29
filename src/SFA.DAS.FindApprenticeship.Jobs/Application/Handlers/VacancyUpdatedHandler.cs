using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain;
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
    private readonly ILogger<VacancyUpdatedHandler> _logger;
    public VacancyUpdatedHandler(IAzureSearchHelper azureSearchHelper, IRecruitService recruitService, IDateTimeService dateTimeService, ILogger<VacancyUpdatedHandler> logger)
    {
        _azureSearchHelperService = azureSearchHelper;
        _recruitService = recruitService;
        _dateTimeService = dateTimeService;
        _logger = logger;
    }

    public async Task Handle(VacancyUpdatedEvent vacancyUpdatedEvent)
    {
        _logger.LogInformation($"Vacancy Updated Event handler invoked at {DateTime.UtcNow}");

        //TODO: uncomment when FAI-1020 is done
        //TODO: will need to get the alias and use that to get the index, so that the document can be fetched.
        //var vacancyReference = $"VAC{vacancyUpdatedEvent.VacancyReference}";
        //var document = await _azureSearchHelperService.GetDocument(indexName, vacancyReference);

        //var updatedVacancy = await _recruitService.GetLiveVacancy(vacancyUpdatedEvent.VacancyId);

        //switch (vacancyUpdatedEvent.UpdateKind)
        //{
        //    case LiveUpdateKind.StartDate:
        //        document.Value.StartDate = updatedVacancy.LiveVacancy.StartDate;
        //        break;
        //    case LiveUpdateKind.ClosingDate:
        //        document.Value.ClosingDate = updatedVacancy.LiveVacancy.ClosingDate;
        //        break;
        //    default:
        //        break;
        //}

        //var uploadBatch = Enumerable.Empty<ApprenticeAzureSearchDocument>().Append(document);
        //await _azureSearchHelperService.UploadDocuments(indexName, uploadBatch);
    }
}
