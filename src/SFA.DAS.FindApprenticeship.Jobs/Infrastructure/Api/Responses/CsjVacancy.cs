using SFA.DAS.FindApprenticeship.Jobs.Domain.Enums;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

public record CsjVacancy : ExternalLiveVacancy
{
    public readonly VacancyDataSource VacancySource = VacancyDataSource.Csj;
}