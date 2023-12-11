﻿#nullable enable
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
    public long VacancyReference { get; set; }
    public string VacancyTitle { get; set; } = null!;
    public int NumberOfPositions { get; set; }
    public string ApprenticeshipTitle { get; set; } = null!;
    public string? Description { get; set; }
    public Address? Address { get; set; }
    public string? EmployerName { get; set; }
    public long? Ukprn { get; set; }
    public string? ProviderName { get; set; }
    public DateTime LiveDate { get; set; }
    public string? ProgrammeId { get; set; }
    public string? ProgrammeType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ClosingDate { get; set; }
    public string Route { get; set; } = null!;
    public int Level { get; set; }
    public Wage? Wage { get; set; }
}

public class Address
{
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? AddressLine3 { get; set; }
    public string? AddressLine4 { get; set; }
    public string? Postcode { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

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
}