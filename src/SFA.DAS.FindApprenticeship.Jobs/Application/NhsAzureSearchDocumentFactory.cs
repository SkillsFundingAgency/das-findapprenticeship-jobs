using Microsoft.Spatial;
using SFA.DAS.Encoding;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.Application;

public interface INhsAzureSearchDocumentFactory
{
    ApprenticeAzureSearchDocument Create(GetNhsLiveVacanciesApiResponse.NhsLiveVacancy vacancy);
}

public class NhsAzureSearchDocumentFactory(IEncodingService encodingService): INhsAzureSearchDocumentFactory
{
    private const string VacancySourceNhs = "NHS";
    
    public ApprenticeAzureSearchDocument Create(GetNhsLiveVacanciesApiResponse.NhsLiveVacancy vacancy)
    {
        var accountId = encodingService.Decode(vacancy.AccountPublicHashedId, EncodingType.PublicAccountId);
        var accountLegalEntityId = encodingService.Decode(vacancy.AccountLegalEntityPublicHashedId, EncodingType.PublicAccountLegalEntityId);
        
        return new ApprenticeAzureSearchDocument
        {
            AccountId = accountId,
            AccountLegalEntityId = accountLegalEntityId,
            AccountLegalEntityPublicHashedId = vacancy.AccountLegalEntityPublicHashedId,
            AccountPublicHashedId = vacancy.AccountPublicHashedId,
            AdditionalQuestion1 = vacancy.AdditionalQuestion1,
            AdditionalQuestion2 = vacancy.AdditionalQuestion2,
            Address = (AddressAzureSearchDocument)vacancy.Address,
            AnonymousEmployerName = vacancy.AnonymousEmployerName,
            ApplicationMethod = vacancy.ApplicationMethod,
            ApplicationUrl = vacancy.ApplicationUrl,
            ApprenticeshipLevel = vacancy.ApprenticeshipLevel,
            ClosingDate = vacancy.ClosingDate,
            Course = (CourseAzureSearchDocument)vacancy,
            Description = vacancy.Description,
            EmployerContactEmail = vacancy.EmployerContactEmail,
            EmployerContactName = vacancy.EmployerContactName,
            EmployerContactPhone = vacancy.EmployerContactPhone,
            EmployerDescription = vacancy.EmployerDescription,
            EmployerName = vacancy.EmployerName,
            EmployerWebsiteUrl = vacancy.EmployerWebsiteUrl,
            HoursPerWeek = (double)vacancy.Wage!.WeeklyHours,
            Id = vacancy.VacancyReference,
            IsDisabilityConfident = vacancy.IsDisabilityConfident,
            IsEmployerAnonymous = vacancy.IsEmployerAnonymous,
            IsPositiveAboutDisability = vacancy.IsPositiveAboutDisability,
            IsPrimaryLocation = true,
            IsRecruitVacancy = vacancy.IsRecruitVacancy,
            Location = GeographyPoint.Create(vacancy.Address!.Latitude, vacancy.Address!.Longitude),
            LongDescription = vacancy.LongDescription,
            NumberOfPositions = vacancy.NumberOfPositions,
            OtherAddresses = [],
            OutcomeDescription = vacancy.OutcomeDescription,
            PostedDate = vacancy.PostedDate,
            ProviderContactEmail = vacancy.ProviderContactEmail,
            ProviderContactName = vacancy.ProviderContactName,
            ProviderContactPhone = vacancy.ProviderContactPhone,
            ProviderName = vacancy.ProviderName,
            Qualifications = vacancy.Qualifications.Select(q => (QualificationAzureSearchDocument)q).ToList(),
            Route = vacancy.Route,
            SearchTags = vacancy.SearchTags,
            Skills = vacancy.Skills.ToList(),
            StartDate = vacancy.StartDate,
            ThingsToConsider = vacancy.ThingsToConsider,
            Title = vacancy.Title,
            TrainingDescription = vacancy.TrainingDescription,
            TypicalJobTitles = vacancy.TypicalJobTitles,
            Ukprn = vacancy.Ukprn.ToString(),
            VacancyLocationType = vacancy.VacancyLocationType,
            VacancyReference = $"{vacancy.VacancyReference}",
            VacancySource = VacancySourceNhs,
            Wage = (WageAzureSearchDocument)vacancy.Wage,
            WageText = vacancy.Wage.WageText,
        };
    }
}