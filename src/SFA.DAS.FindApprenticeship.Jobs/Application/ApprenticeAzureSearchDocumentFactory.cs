using Microsoft.Spatial;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.Application;

public static class ApprenticeAzureSearchDocumentFactory
{
    private const string VacancySourceRecruit = "RAA";
    
    // IMPORTANT: this method replaces the implicit cast function on the ApprenticeAzureSearchDocument
    // as we now have a 1 to MANY relationship between live vacancies and azure indexed documents.
    public static IEnumerable<ApprenticeAzureSearchDocument> Create(LiveVacancy vacancy)
    {
        switch (vacancy.EmploymentLocationOption)
        {
            case AvailableWhere.OneLocation:
            {
                var address = vacancy.EmploymentLocations![0];
                var document = MapWithoutAddress(vacancy);
                document.Address = (AddressAzureSearchDocument)address;
                document.Location = GeographyPoint.Create(address.Latitude, address.Longitude);
                return [document];
            }
            case AvailableWhere.MultipleLocations:
            {
                var results = new List<ApprenticeAzureSearchDocument>();
                var count = 0;
                foreach (var address in vacancy.EmploymentLocations!)
                {
                    var document = MapWithoutAddress(vacancy);
                    document.Address = (AddressAzureSearchDocument)address;
                    document.Location = GeographyPoint.Create(address.Latitude, address.Longitude);
                    document.OtherAddresses = vacancy.EmploymentLocations.Except([address]).Select(x => (AddressAzureSearchDocument)x).ToList();
                    document.Id = count++ == 0 ? document.Id : $"{document.Id}-{count}";  
                    results.Add(document);
                }
                return results;
            }
            case AvailableWhere.AcrossEngland:
            {
                var document = MapWithoutAddress(vacancy);
                document.EmploymentLocationInformation = vacancy.EmploymentLocationInformation;
                return [document];
            }
            default:
            {
                var document = MapWithoutAddress(vacancy);
                document.Address = (AddressAzureSearchDocument)vacancy.Address;
                document.Location = GeographyPoint.Create(vacancy.Address!.Latitude, vacancy.Address.Longitude);
                return [document];
            }
        }
    }
    
    private static ApprenticeAzureSearchDocument MapWithoutAddress(LiveVacancy vacancy)
    {
        return new ApprenticeAzureSearchDocument
        {
            AccountLegalEntityPublicHashedId = vacancy.AccountLegalEntityPublicHashedId,
            AccountPublicHashedId = vacancy.AccountPublicHashedId,
            AdditionalQuestion1 = vacancy.AdditionalQuestion1,
            AdditionalQuestion2 = vacancy.AdditionalQuestion2,
            AdditionalTrainingDescription = vacancy.AdditionalTrainingDescription,
            AnonymousEmployerName = vacancy.AnonymousEmployerName,
            ApplicationInstructions = vacancy.ApplicationInstructions,
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
            Id = vacancy.Id,
            IsDisabilityConfident = vacancy.IsDisabilityConfident,
            IsEmployerAnonymous = vacancy.IsEmployerAnonymous,
            IsPositiveAboutDisability = vacancy.IsPositiveAboutDisability,
            IsPrimaryLocation = vacancy.IsPrimaryLocation,
            IsRecruitVacancy = vacancy.IsRecruitVacancy,
            LongDescription = vacancy.LongDescription,
            NumberOfPositions = vacancy.NumberOfPositions,
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
            VacancyReference = $"VAC{vacancy.VacancyReference}",
            VacancySource = VacancySourceRecruit,
            WageText = vacancy.Wage.WageText,
            Wage = (WageAzureSearchDocument)vacancy.Wage,
        };
    }

}