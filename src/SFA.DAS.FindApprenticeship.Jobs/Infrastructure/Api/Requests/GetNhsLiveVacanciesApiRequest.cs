using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;
public class GetNhsLiveVacanciesApiRequest : IGetApiRequest
{
    public string GetUrl => $"nhsvacancies";
}