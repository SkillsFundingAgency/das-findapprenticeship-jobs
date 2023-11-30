using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;
public class GetLiveVacancyApiRequest : IGetApiRequest
{
    private readonly long _vacancyReference;

    public GetLiveVacancyApiRequest(long vacancyReference)
    {
        _vacancyReference = vacancyReference;
    }

    public string GetUrl => $"livevacancy?vacancyRef={_vacancyReference}";
}