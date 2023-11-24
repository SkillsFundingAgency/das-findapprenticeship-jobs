using System.Linq;
using System.Threading.Tasks;
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
    public VacancyUpdatedHandler(IAzureSearchHelper azureSearchHelper, IRecruitService recruitService, IDateTimeService dateTimeService)
    {
        _azureSearchHelperService = azureSearchHelper;
        _recruitService = recruitService;
        _dateTimeService = dateTimeService;
    }

    public async Task Handle(VacancyUpdatedEvent vacancyUpdatedEvent)
    {
        var indexName = $"{Constants.IndexPrefix}{_dateTimeService.GetCurrentDateTime().ToString(Constants.IndexDateSuffixFormat)}";
        var vacancyReference = $"VAC{vacancyUpdatedEvent.VacancyReference}";
        var document = await _azureSearchHelperService.GetDocument(indexName, vacancyReference);

        var updatedVacancy = await _recruitService.GetLiveVacancy(vacancyUpdatedEvent.VacancyId);

        switch (vacancyUpdatedEvent.UpdateKind)
        {
            case LiveUpdateKind.StartDate:
                document.Value.StartDate = updatedVacancy.LiveVacancy.StartDate;
                break;
            case LiveUpdateKind.ClosingDate:
                document.Value.ClosingDate = updatedVacancy.LiveVacancy.ClosingDate;
                break;
            default:
                break;
        }

        var uploadBatch = Enumerable.Empty<ApprenticeAzureSearchDocument>().Append(document);
        await _azureSearchHelperService.UploadDocuments(indexName, uploadBatch);
    }
}
