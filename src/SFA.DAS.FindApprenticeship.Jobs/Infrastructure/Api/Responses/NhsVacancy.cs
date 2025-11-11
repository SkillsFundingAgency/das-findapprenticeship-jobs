namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

public record NhsVacancy : ExternalLiveVacancy
{
    public const string VacancySource = "NHS";
}