using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;
public class GetLiveVacancyApiRequest : IGetApiRequest
{
    private readonly long _vacancyRef;

    public GetLiveVacancyApiRequest(long vacancyRef)
    {
        _vacancyRef = vacancyRef;
    }

    public string GetUrl => $"livevacancy?vacancyRef={_vacancyRef}";
}
