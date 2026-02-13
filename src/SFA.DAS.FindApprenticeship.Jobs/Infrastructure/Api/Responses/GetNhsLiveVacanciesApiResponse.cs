namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;
public class GetNhsLiveVacanciesApiResponse
{
    public IEnumerable<NhsVacancy> Vacancies { get; init; } = null!;
    public int PageSize { get; set; }
    public int PageNo { get; set; }
    public int TotalLiveVacanciesReturned { get; set; }
    public int TotalLiveVacancies { get; set; }
    public int TotalPages { get; set; }
}