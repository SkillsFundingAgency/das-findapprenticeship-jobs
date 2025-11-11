namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

public record CsjVacancy : ExternalLiveVacancy
{
    public const string VacancySource = "CSJ";
}