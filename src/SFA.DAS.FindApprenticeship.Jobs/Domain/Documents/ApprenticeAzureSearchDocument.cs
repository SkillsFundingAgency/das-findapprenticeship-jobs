﻿#nullable enable
using Azure.Search.Documents.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
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
            ApprenticeshipLevel = source.ApprenticeshipLevel,
            ApplicationMethod = source.ApplicationMethod,
            ApplicationUrl = source.ApplicationUrl,
            AccountPublicHashedId = source.AccountPublicHashedId,
            AccountLegalEntityPublicHashedId = source.AccountLegalEntityPublicHashedId,
            HoursPerWeek = (long)source.Wage!.WeeklyHours,
            ProviderName = source.ProviderName,
            StartDate = source.StartDate,
            PostedDate = source.PostedDate,
            ClosingDate = source.ClosingDate,
            Title = source.Title,
            Ukprn = source.Ukprn.ToString(),
            VacancyReference = $"VAC{source.VacancyReference}",
            Wage = (WageAzureSearchDocument)source.Wage,
            Course = (CourseAzureSearchDocument)source,
            Address = (AddressAzureSearchDocument)source.Address,
            Location = GeographyPoint.Create(source.Address!.Latitude, source.Address!.Longitude),
            NumberOfPositions = source.NumberOfPositions,
            LongDescription = source.LongDescription,
            TrainingDescription = source.TrainingDescription,
            OutcomeDescription = source.OutcomeDescription,
            Skills = source.Skills.ToList(),
            ThingsToConsider = source.ThingsToConsider,
            Id = source.Id,
            AnonymousEmployerName = source.AnonymousEmployerName,
            IsDisabilityConfident = source.IsDisabilityConfident,
            IsPositiveAboutDisability = source.IsPositiveAboutDisability,
            IsEmployerAnonymous = source.IsEmployerAnonymous,
            IsRecruitVacancy = source.IsRecruitVacancy,
            VacancyLocationType = source.VacancyLocationType,
            EmployerContactEmail = source.EmployerContactEmail,
            EmployerContactName = source.EmployerContactName,
            EmployerContactPhone = source.EmployerContactPhone,
            EmployerDescription = source.EmployerDescription,
            EmployerWebsiteUrl = source.EmployerWebsiteUrl,
            Qualifications = source.Qualifications.Select(q => (QualificationAzureSearchDocument)q).ToList()
        };
    }

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string? Description { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = false, IsFacetable = true)]
    public string Route { get; set; } = null!;

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string? EmployerName { get; set; }

    [SimpleField(IsFilterable = true)]
    public string AccountPublicHashedId { get; set; } = null!;

    [SimpleField(IsFilterable = true)]
    public string AccountLegalEntityPublicHashedId { get; set; } = null!;

    [SimpleField(IsFilterable = true)]
    public string ApprenticeshipLevel { get; set; } = null!;

    [SimpleField]
    public string ApplicationMethod { get; set; } = null!;

    [SimpleField]
    public string? ApplicationUrl { get; set; }

    [SimpleField]
    public long HoursPerWeek { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string? ProviderName { get; set; }

    [SimpleField]
    public DateTimeOffset StartDate { get; set; }

    [SimpleField(IsSortable = true)]
    public DateTimeOffset PostedDate { get; set; }

    [SimpleField]
    public DateTimeOffset ClosingDate { get; set; }

    [SimpleField]
    public long NumberOfPositions { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string? Title { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string Ukprn { get; set; }

    [SimpleField(IsFilterable = true)]
    public string VacancyReference { get; set; } = null!;

    [SimpleField(IsKey = true)]
    public string Id { get; set; } = null!;

    [SearchableField]
    public CourseAzureSearchDocument? Course { get; set; }

    [SearchableField]
    public AddressAzureSearchDocument? Address { get; set; }

    [SearchableField]
    public WageAzureSearchDocument? Wage { get; set; }

    [System.Text.Json.Serialization.JsonConverter(typeof(MicrosoftSpatialGeoJsonConverter))]
    [SimpleField(IsSortable = true, IsFilterable = true)]
    public GeographyPoint? Location { get; set; }

    [SearchableField]
    public string? LongDescription { get; set; }

    [SearchableField]
    public string? OutcomeDescription { get; set; }

    [SearchableField]
    public string? TrainingDescription { get; set; }

    [SearchableField] public List<string> Skills { get; set; } = null!;

    [SimpleField]
    public List<QualificationAzureSearchDocument> Qualifications { get; set; } = null!;

    [SearchableField]
    public string? ThingsToConsider { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string? AnonymousEmployerName { get; set; }

    [SimpleField]
    public bool IsDisabilityConfident { get; set; }

    [SimpleField]
    public bool IsPositiveAboutDisability { get; set; }

    [SimpleField]
    public bool IsEmployerAnonymous { get; set; }

    [SimpleField]
    public bool IsRecruitVacancy { get; set; }

    [SimpleField]
    public string? VacancyLocationType { get; set; }
    
    [SearchableField]
    public string? EmployerDescription { get; set; }

    [SimpleField]
    public string? EmployerWebsiteUrl { get; set; }

    [SimpleField]
    public string? EmployerContactPhone { get; set; }

    [SimpleField]
    public string? EmployerContactEmail { get; set; }

    [SimpleField]
    public string? EmployerContactName { get; set; }
}

public class CourseAzureSearchDocument
{
    public static implicit operator CourseAzureSearchDocument(LiveVacancy source)
    {
        return new CourseAzureSearchDocument
        {
            Level = source.Level,
            Title = source.ApprenticeshipTitle,
            LarsCode = source.StandardLarsCode,
            RouteCode = source.RouteCode
        };
    }

    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public int? LarsCode { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string? Title { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public long Level { get; set; }

    [SimpleField]
    public int RouteCode { get; set; }
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
            Postcode = source.Postcode,
            Longitude = source.Longitude,
            Latitude = source.Latitude
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

    [SimpleField]
    public double Latitude { get; set; }

    [SimpleField]
    public double Longitude { get; set; }

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
            Duration = source.Duration,
            WageAdditionalInformation = source.WageAdditionalInformation,
            WorkingWeekDescription = source.WorkingWeekDescription,
            ApprenticeMinimumWage = (double) (source.ApprenticeMinimumWage ?? 0),
            Under18NationalMinimumWage = (double)(source.Under18NationalMinimumWage ?? 0),
            Between18AndUnder21NationalMinimumWage = (double)(source.Between18AndUnder21NationalMinimumWage ?? 0),
            Between21AndUnder25NationalMinimumWage = (double)(source.Between21AndUnder25NationalMinimumWage ?? 0),
            Over25NationalMinimumWage = (double)(source.Over25NationalMinimumWage ?? 0)
        };
    }

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string? WageAdditionalInformation { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string? WageType { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string? WorkingWeekDescription { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public int Duration { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string? WageUnit { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public long? WageAmount { get; set; }

    [SimpleField(IsSortable = true)]
    public double ApprenticeMinimumWage { get; set; }

    [SimpleField(IsSortable = true)]
    public double Under18NationalMinimumWage { get; set; }

    [SimpleField(IsSortable = true)]
    public double Between18AndUnder21NationalMinimumWage { get; set; }

    [SimpleField(IsSortable = true)]
    public double Between21AndUnder25NationalMinimumWage { get; set; }

    [SimpleField(IsSortable = true)]
    public double Over25NationalMinimumWage { get; set; }
}

public class QualificationAzureSearchDocument
{
    public static implicit operator QualificationAzureSearchDocument(Qualification source)
    {
        return new QualificationAzureSearchDocument
        {
            QualificationType = source.QualificationType,
            Grade = source.Grade,
            Subject = source.Subject,
            Weighting = source.Weighting
        };
    }

    [SimpleField]
    public string? QualificationType { get; set; }

    [SimpleField]
    public string? Subject { get; set; }

    [SimpleField]
    public string? Grade { get; set; }
    
    [SimpleField]
    public string? Weighting { get; set; }
}
