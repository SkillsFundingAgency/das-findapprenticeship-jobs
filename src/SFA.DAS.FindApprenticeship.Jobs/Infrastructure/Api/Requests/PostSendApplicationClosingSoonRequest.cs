using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;

public class PostSendApplicationClosingSoonRequest : IPostApiRequest
{
    private readonly long _vacancyRef;
    private readonly int _daysUntilClosing;

    public PostSendApplicationClosingSoonRequest(long vacancyRef, int daysUntilClosing)
    {
        _vacancyRef = vacancyRef;
        _daysUntilClosing = daysUntilClosing;
    }

    public string PostUrl => $"livevacancies/{_vacancyRef}?daysUntilClosing={_daysUntilClosing}";
}