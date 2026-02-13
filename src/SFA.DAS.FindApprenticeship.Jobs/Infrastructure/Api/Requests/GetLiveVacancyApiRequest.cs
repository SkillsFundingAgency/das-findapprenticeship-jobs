using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;
public class GetLiveVacancyApiRequest : IGetApiRequest
{
    private readonly string _vacancyReference;

    public GetLiveVacancyApiRequest(string vacancyReference)
    {
        _vacancyReference = vacancyReference;
    }

    public string GetUrl => $"livevacancies/{_vacancyReference}";
}