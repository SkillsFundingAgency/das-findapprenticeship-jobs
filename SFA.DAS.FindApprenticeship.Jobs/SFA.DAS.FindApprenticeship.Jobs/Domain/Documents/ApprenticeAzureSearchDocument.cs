﻿#nullable enable
using Azure.Search.Documents.Indexes;
using System;
using Microsoft.Spatial;
using Azure.Core.Serialization;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
public class ApprenticeAzureSearchDocument
{
    public static implicit operator ApprenticeAzureSearchDocument(LiveVacancy source)
    {
        return new ApprenticeAzureSearchDocument
        {
            Description = source.Description,
            Route = source.Route,
            EmployerName = source.EmployerName,
            HoursPerWeek = (long)source.Wage!.WeeklyHours,
            ProviderName = source.ProviderName,
            StartDate = source.StartDate,
            PostedDate = source.LiveDate,
            ClosingDate = source.ClosingDate,
            Title = source.VacancyTitle,
            Ukprn = source.ProviderId,
            VacancyReference = $"VAC{source.VacancyId}",
            Wage = (WageAzureSearchDocument)source.Wage,
            Course = (CourseAzureSearchDocument)source,
            Address = (AddressAzureSearchDocument)source.EmployerLocation,
            Location = GeographyPoint.Create(source.EmployerLocation!.Latitude, source.EmployerLocation!.Longitude),
            // Use for 'vacancies' index:
            NumberOfPositions = 2
        };
    }

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string? Description { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = false, IsFacetable = true)]
    public string Route { get; set; } = null!;

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string? EmployerName { get; set; }

    [SimpleField]
    public long HoursPerWeek { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string? ProviderName { get; set; }

    [SimpleField]
    public DateTimeOffset StartDate { get; set; }

    [SimpleField]
    public DateTimeOffset PostedDate { get; set; }

    [SimpleField]
    public DateTimeOffset ClosingDate { get; set; }

    // Use for 'vacancies' index:
    [SimpleField]
    public long NumberOfPositions { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string? Title { get; set; }

    [SimpleField]
    public long? Ukprn { get; set; }

    [SimpleField(IsKey = true, IsFilterable = true)]
    public string VacancyReference { get; set; } = null!;

    [SearchableField]
    public CourseAzureSearchDocument? Course { get; set; }

    [SearchableField]
    public AddressAzureSearchDocument? Address { get; set; }

    [SearchableField]
    public WageAzureSearchDocument? Wage { get; set; }

    [System.Text.Json.Serialization.JsonConverter(typeof(MicrosoftSpatialGeoJsonConverter))]
    [SimpleField(IsSortable = true, IsFilterable = true)]
    public GeographyPoint? Location { get; set; }
}

public class CourseAzureSearchDocument
{
    public static implicit operator CourseAzureSearchDocument(LiveVacancy source)
    {
        return new CourseAzureSearchDocument
        {
            Level = source.Level,
            Title = source.ApprenticeshipTitle,
            LarsCode = (long)Convert.ToDouble(source.ProgrammeId)
        };
    }

    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public long LarsCode { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string? Title { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public long Level { get; set; }
}

public class AddressAzureSearchDocument
{
    public static implicit operator AddressAzureSearchDocument(Address? source)
    {
        return new AddressAzureSearchDocument
        {
            AddressLine1 = source.AddressLine1,
            AddressLine2 = source.AddressLine2,
            AddressLine3 = source.AddressLine3,
            AddressLine4 = source.AddressLine4,
            Postcode = source.Postcode
        };
    }

    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string? AddressLine1 { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string? AddressLine2 { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string? AddressLine3 { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string? AddressLine4 { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string? Postcode { get; set; }

}

public class WageAzureSearchDocument
{
    public static implicit operator WageAzureSearchDocument(Wage source)
    {
        return new WageAzureSearchDocument
        {
            WageAmount = (long?)source.FixedWageYearlyAmount ?? null,
            WageType = source.WageType,
            WageUnit = source.DurationUnit,
            WageAdditionalInformation = source.WageAdditionalInformation,
            WorkingWeekDescription = source.WorkingWeekDescription
        };
    }

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string? WageAdditionalInformation { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string? WageType { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string? WorkingWeekDescription { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string? WageUnit { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public long? WageAmount { get; set; }
}
