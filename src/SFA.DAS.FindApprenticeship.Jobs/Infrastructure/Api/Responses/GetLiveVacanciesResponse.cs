#nullable enable
using System.Text.Json.Serialization;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;
public class GetLiveVacanciesApiResponse
{
    public IEnumerable<LiveVacancy> Vacancies { get; set; } = null!;
    public int PageSize { get; set; }
    public int PageNo { get; set; }
    public int TotalLiveVacanciesReturned { get; set; }
    public int TotalLiveVacancies { get; set; }
    public int TotalPages { get; set; }
}
public class LiveVacancy
{
    public Guid VacancyId { get; set; }
    public string VacancyReference { get; set; }
    public string Title { get; set; } = null!;
    public int NumberOfPositions { get; set; }
    public string ApprenticeshipTitle { get; set; } = null!;
    public string? Description { get; set; }
    public Address? Address { get; set; }
    public List<Address>? EmploymentLocations { get; set; }
    public List<Address>? OtherAddresses { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter<AvailableWhere>))]
    public AvailableWhere? EmploymentLocationOption { get; set; }
    public string? EmploymentLocationInformation { get; set; }
    public string? EmployerName { get; set; }
    public string ApprenticeshipLevel { get; set; } = null!;
    public ApprenticeshipTypes? ApprenticeshipType { get; set; }
    public string ApplicationMethod { get; set; } = null!;
    public string? ApplicationUrl { get; set; }
    public string? ApplicationInstructions { get; set; }
    public long AccountId { get; set; }
    public long AccountLegalEntityId { get; set; }
    public long Ukprn { get; set; }
    public string? ProviderName { get; set; }
    public DateTime PostedDate { get; set; }
    public int? StandardLarsCode { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ClosingDate { get; set; }
    public int RouteCode { get; set; }
    public string Route { get; set; } = null!;
    public int Level { get; set; }
    public Wage Wage { get; set; } = null!;
    public string? LongDescription { get; set; }
    public string? OutcomeDescription { get; set; }
    public string? TrainingDescription { get; set; }
    public IEnumerable<string>? Skills { get; set; }
    public IEnumerable<Qualification>? Qualifications { get; set; }
    public string? ThingsToConsider { get; set; }
    public string Id { get; set; } = null!;
    public bool IsDisabilityConfident { get; set; }
    public bool IsEmployerAnonymous { get; set; }
    public string? AnonymousEmployerName { get; set; }
    public bool IsRecruitVacancy { get; set; }
    public string? VacancyLocationType { get; set; }
    public string? EmployerDescription { get; set; }
    public string? EmployerWebsiteUrl { get; set; }
    public string? EmployerContactPhone { get; set; }
    public string? EmployerContactEmail { get; set; }
    public string? EmployerContactName { get; set; }
    public string? ProviderContactEmail { get; set; }
    public string? ProviderContactName { get; set; }
    public string? ProviderContactPhone { get; set; }
    public bool IsPositiveAboutDisability { get; set; }
    public string TypicalJobTitles { get; set; } = null!;
    public string? AdditionalQuestion1 { get; set; }
    public string? AdditionalQuestion2 { get; set; }
    public string? AdditionalTrainingDescription { get; set; }
    public string? SearchTags { get; set; }
}

public class Address
{
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? AddressLine3 { get; set; }
    public string? AddressLine4 { get; set; }
    public string? Postcode { get; set; }
    public double? Latitude { get; set; } = 0.00;
    public double? Longitude { get; set; } = 0.00;
    public string? Country { get; set; }
}

public class Wage
{
    public int Duration { get; set; }
    public string? DurationUnit { get; set; }
    public decimal? FixedWageYearlyAmount { get; set; }
    public string? WageAdditionalInformation { get; set; }
    public string? WageType { get; set; }
    public decimal WeeklyHours { get; set; }
    public string? WorkingWeekDescription { get; set; }
    public decimal? ApprenticeMinimumWage { get; set; }
    public decimal? Under18NationalMinimumWage { get; set; }
    public decimal? Between18AndUnder21NationalMinimumWage { get; set; }
    public decimal? Between21AndUnder25NationalMinimumWage { get; set; }
    public decimal? Over25NationalMinimumWage { get; set; }
    public string WageText { get; set; } = null!;
    public string? CompanyBenefitsInformation { get; set; }

}

public class Qualification
{
    public string QualificationType { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Grade { get; set; } = null!;
    public string Weighting { get; set; } = null!;
}

public enum AvailableWhere
{
    OneLocation,
    MultipleLocations,
    AcrossEngland,
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ApprenticeshipTypes
{
    Standard,
    Foundation,
}