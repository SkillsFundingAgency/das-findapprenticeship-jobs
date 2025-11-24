using SFA.DAS.FindApprenticeship.Jobs.Domain.Enums;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

public record NhsVacancy : ExternalLiveVacancy
{
    public const VacancyDataSource VacancySource = VacancyDataSource.Nhs;
}