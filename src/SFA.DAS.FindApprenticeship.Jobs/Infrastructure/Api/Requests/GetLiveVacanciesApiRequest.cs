using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;
public class GetLiveVacanciesApiRequest : IGetApiRequest
{
    private readonly int? _pageNumber;
    private readonly int? _pageSize;

    public GetLiveVacanciesApiRequest(int? pageNumber, int? pageSize)
    {
        _pageNumber = pageNumber;
        _pageSize = pageSize;
    }

    public string GetUrl => $"livevacancies?pageSize={_pageSize}&pageNo={_pageNumber}";
}
