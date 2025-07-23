namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;
public class GetCivilServiceLiveVacanciesApiResponse
{
    public IEnumerable<ExternalLiveVacancy> Vacancies { get; set; } = null!;
    public int PageSize { get; set; }
    public int PageNo { get; set; }
    public int TotalLiveVacanciesReturned { get; set; }
    public int TotalLiveVacancies { get; set; }
    public int TotalPages { get; set; }
}