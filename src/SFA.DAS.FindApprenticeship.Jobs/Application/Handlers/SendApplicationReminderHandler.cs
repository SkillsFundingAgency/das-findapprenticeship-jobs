using System.Threading.Tasks;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;

public class SendApplicationReminderHandler : ISendApplicationReminderHandler
{
    private readonly IRecruitService _recruitService;

    public SendApplicationReminderHandler(IRecruitService recruitService)
    {
        _recruitService = recruitService;
    }
    
    public async Task Handle(long vacancyRef, int daysUntilExpiry)
    {
        await _recruitService.SendApplicationClosingSoonReminder(vacancyRef, daysUntilExpiry);
    }
}