using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.FindApprenticeship.Jobs.Domain;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
public class RecruitIndexerJobHandler : IRecruitIndexerJobHandler
{
    private readonly IRecruitService _recruitService;
    private readonly IAzureSearchHelper _azureSearchHelperService;
    private readonly IDateTimeService _dateTimeService;
    private const int PageNo = 1;
    private const int PageSize = 500;

    public RecruitIndexerJobHandler(IRecruitService recruitService, IAzureSearchHelper azureSearchHelperService, IDateTimeService dateTimeService)
    {
        _recruitService = recruitService;
        _azureSearchHelperService = azureSearchHelperService;
        _dateTimeService = dateTimeService;
    }

    public async Task Handle()
    {
        var liveVacancies = await _recruitService.GetLiveVacancies(PageNo, PageSize);
        var hasData = liveVacancies != null && liveVacancies.Vacancies.Any();
        if (hasData)
        {
            var indexName = $"{Constants.IndexPrefix}{_dateTimeService.GetCurrentDateTime().ToString(Constants.IndexDateSuffixFormat)}";

            await _azureSearchHelperService.CreateIndex(indexName);
            var batchDocuments = liveVacancies.Vacancies.Select(a => (ApprenticeAzureSearchDocument)a).ToList();
            await _azureSearchHelperService.UploadDocuments(indexName, batchDocuments);
            await _azureSearchHelperService.UpdateAlias(Constants.AliasName, indexName);
        }
    }
}
