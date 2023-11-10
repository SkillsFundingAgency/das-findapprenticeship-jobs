#nullable enable
using System.Collections.Generic;
using System;

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
    public string VacancyTitle { get; set; } = null!;
    public string ApprenticeshipTitle { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Address EmployerLocation { get; set; } = null!;
    public string EmployerName { get; set; } = null!;
    public long ProviderId { get; set; }
    public string ProviderName { get; set; } = null!;
    public DateTime LiveDate { get; set; }
    public string ProgrammeId { get; set; } = null!;
    public string ProgrammeType { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime ClosingDate { get; set; }
    public int? RouteId { get; set; }
    public Wage Wage { get; set; } = null!;
}

public class Address
{
    public string AddressLine1 { get; set; } = null!;
    public string AddressLine2 { get; set; } = null!;
    public string AddressLine3 { get; set; } = null!;
    public string AddressLine4 { get; set; } = null!;
    public string Postcode { get; set; } = null!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }

}

public class Wage
{
    public int Duration { get; set; }
    public string DurationUnit { get; set; } = null!;
    public decimal? FixedWageYearlyAmount { get; set; }
    public string WageAdditionalInformation { get; set; } = null!;
    public string WageType { get; set; } = null!;
    public decimal WeeklyHours { get; set; }
    public string WorkingWeekDescription { get; set; } = null!;
}