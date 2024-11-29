using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;
public class GetLiveVacanciesApiRequest : IGetApiRequest
{
    public GetLiveVacanciesApiRequest(int? pageNumber, int? pageSize, DateTime? closingDate)
    {
        var getUrl = $"livevacancies?pageSize={pageSize}&pageNo={pageNumber}";
        if (closingDate != null)
        {
            getUrl += $"&closingDate={closingDate.Value.Date}";
        }
        GetUrl = getUrl;
    }

    public string GetUrl { get; }
}
