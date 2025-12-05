using SFA.DAS.FindApprenticeship.Jobs.Domain.Enums;
using System.Text.Json.Serialization;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

public record CsjVacancy : ExternalLiveVacancy
{
    public const VacancyDataSource VacancySource = VacancyDataSource.Csj;

    [JsonConverter(typeof(JsonStringEnumConverter<AvailableWhere>))]
    public AvailableWhere? EmploymentLocationOption { get; set; }
}