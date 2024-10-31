using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;

public class VacancyClosingSoonHandler : IVacancyClosingSoonHandler
{
    private readonly IFindApprenticeshipJobsService _recruitService;
    private readonly IDateTimeService _dateTimeService;
    private const int PageSize = 500;

    public VacancyClosingSoonHandler(IFindApprenticeshipJobsService recruitService, IDateTimeService dateTimeService)
    {
        _recruitService = recruitService;
        _dateTimeService = dateTimeService;
    }
    public async Task<IList<long>> Handle(int daysToExpire)
    {
        var dateToUse = _dateTimeService.GetCurrentDateTime().AddDays(daysToExpire);
        
        var pageNo = 1;
        var totalPages = 100;
        var vacancyReferences = new List<long>();
        while (pageNo <= totalPages)
        {
            var liveVacancies = await _recruitService.GetLiveVacancies(pageNo, PageSize, dateToUse);
            

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