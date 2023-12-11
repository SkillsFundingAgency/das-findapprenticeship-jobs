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
    public long VacancyReference { get; set; }
    public string Title { get; set; } = null!;
    public int NumberOfPositions { get; set; }
    public string ApprenticeshipTitle { get; set; } = null!;
    public string? Description { get; set; }
    public Address? Address { get; set; }
    public string? EmployerName { get; set; }
    public long? Ukprn { get; set; }
    public string? ProviderName { get; set; }
    public DateTime LiveDate { get; set; }
    public int? StandardLarsCode { get; set; }
    public string? ProgrammeType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ClosingDate { get; set; }
    public string Route { get; set; } = null!;
    public int Level { get; set; }
    public Wage? Wage { get; set; }
    public string LongDescription { get; set; }
    public string OutcomeDescription { get; set; }
    public string TrainingDescription { get; set; }
    public IEnumerable<string> Skills { get; set; }
    //qualifications here
    public string ThingsToConsider { get; set; }
    public string Id { get; set; }
    public bool IsDisabilityConfident { get; set; }
    public bool IsEmployerAnonymous { get; set; }
    public string AnonymousEmployerName { get; set; }
    public bool IsRecruitVacancy { get; set; }
    public string VacancyLocationType { get; set; }
    public string EmployerDescription { get; set; }
    public string EmployerWebsiteUrl { get; set; }
    public string EmployerContactPhone { get; set; }
    public string EmployerContactEmail { get; set; }
    public string EmployerContactName { get; set; }

    //...

    public IEnumerable<Qualification> Qualifications { get; set; } //parser error - reinstate and fix the weighting
    //public string Category { get; set; } //not wanted, remove from outer
    //public string CategoryCode { get; set; } //not wanted, remove from outer
    public bool IsPositiveAboutDisability { get; set; } //can this come out?
    
    //public string SubCategory { get; set; }; //not wanted, remove from outer
    //public string SubCategoryCode { get; set; } //not wanted, remove from outer
    //also remove programmeType from outer
    
    //public long WageAmountLowerBand { get; set; } //remove these 3 in outer
    //public long WageAmountUpperBand { get; set; }//remove
    //public int ExpectedDuration { get; set; }//removed
    //public int Distance { get; set; } //remove
    //public int Score { get; set; }

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

public class Qualification
{
    public string? QualificationType { get; set; }
    public string? Subject { get; set; }
    public string? Grade { get; set; } 
    //public QualificationWeighting? Weighting { get; set; } add this back in
}

public enum QualificationWeighting
{
    Essential,
    Desired
}