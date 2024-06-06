using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;

public class PostVacancyClosedEarlyRequest : IPostApiRequest
{
    private readonly long _vacancyRef;

    public PostVacancyClosedEarlyRequest(long vacancyRef)
    {
        _vacancyRef = vacancyRef;
    }
    public string PostUrl => $"livevacancies/{_vacancyRef}/close";
}