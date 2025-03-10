using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;

public class VacancyClosingSoonHandler(IFindApprenticeshipJobsService findApprenticeshipJobsService, IDateTimeService dateTimeService)
    : IVacancyClosingSoonHandler
{
    private const int PageSize = 100;

    public async Task<IList<long>> Handle(int daysToExpire)
    {
        var dateToUse = dateTimeService.GetCurrentDateTime().AddDays(daysToExpire);
        
        var pageNo = 1;
        var totalPages = 100;
        var vacancyReferences = new List<long>();
        while (pageNo <= totalPages)
        {
            var liveVacancies = await findApprenticeshipJobsService.GetLiveVacancies(pageNo, PageSize, dateToUse);
            

            totalPages = liveVacancies?.TotalPages ?? 0;

            if (liveVacancies != null)
            {
                if (liveVacancies.Vacancies.Any())
                {
                    vacancyReferences = liveVacancies.Vacancies.Select(a => Convert.ToInt64(a.VacancyReference.Replace("VAC",""))).ToList();
                }
                pageNo++;
            }
            else
            {
                break;
            }
        }
        return vacancyReferences;
    }
}