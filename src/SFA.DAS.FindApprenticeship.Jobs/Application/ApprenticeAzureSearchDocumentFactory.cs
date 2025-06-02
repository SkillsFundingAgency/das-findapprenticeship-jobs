using Microsoft.Spatial;
using SFA.DAS.Encoding;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.Application;

public interface IApprenticeAzureSearchDocumentFactory
{
    IEnumerable<ApprenticeAzureSearchDocument> Create(LiveVacancy vacancy);
}

public class ApprenticeAzureSearchDocumentFactory(IEncodingService encodingService): IApprenticeAzureSearchDocumentFactory
{
    private const string VacancySourceRecruit = "RAA";
    
    // IMPORTANT: this method replaces the implicit cast function on the ApprenticeAzureSearchDocument
    // as we now have a 1 to MANY relationship between live vacancies and azure indexed documents.
    public IEnumerable<ApprenticeAzureSearchDocument> Create(LiveVacancy vacancy)
    {
        switch (vacancy.EmploymentLocationOption)
        {
            case AvailableWhere.OneLocation:
            {
                var address = vacancy.EmploymentLocations![0];
                var document = MapWithoutAddress(vacancy);
                document.Address = (AddressAzureSearchDocument)address;
                document.IsPrimaryLocation = true;
                document.Location = GeographyPoint.Create(address.Latitude, address.Longitude);
                return [document];
            }
            case AvailableWhere.MultipleLocations:
            {
                var results = new List<ApprenticeAzureSearchDocument>();
                var locations = vacancy.EmploymentLocations!.DistinctBy(FlattenAddress).ToList();
                var count = 0;
                foreach (var address in locations)
                {
                    var document = MapWithoutAddress(vacancy);
                    document.Address = (AddressAzureSearchDocument)address;
                    document.Id = count++ == 0 ? document.Id : $"{document.Id}-{count}";
                    document.IsPrimaryLocation = count == 1;
                    document.Location = GeographyPoint.Create(address.Latitude, address.Longitude);
                    document.OtherAddresses = locations.Except([address]).Select(OtherAddressAzureSearchDocument.From).ToList();
                    results.Add(document);
                }
                return results;
            }
            case AvailableWhere.AcrossEngland:
            {
                var document = MapWithoutAddress(vacancy);
                document.EmploymentLocationInformation = vacancy.EmploymentLocationInformation;
                document.IsPrimaryLocation = true;
                return [document];
            }
            default:
            {
                var document = MapWithoutAddress(vacancy);
                document.Address = (AddressAzureSearchDocument)vacancy.Address;
                document.IsPrimaryLocation = true;
                document.Location = GeographyPoint.Create(vacancy.Address!.Latitude, vacancy.Address.Longitude);
                return [document];
            }
        }
    }
    
    private ApprenticeAzureSearchDocument MapWithoutAddress(LiveVacancy vacancy)
    {
        var accountId = encodingService.Decode(vacancy.AccountPublicHashedId, EncodingType.AccountId);
        var accountLegalEntityId = encodingService.Decode(vacancy.AccountLegalEntityPublicHashedId, EncodingType.PublicAccountLegalEntityId);
        
        return new ApprenticeAzureSearchDocument
        {
            AccountId = accountId,
            AccountLegalEntityId = accountLegalEntityId,
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
            ApprenticeshipType = vacancy.ApprenticeshipType?.ToString() ?? nameof(ApprenticeshipTypes.Standard),
            AvailableWhere = vacancy.EmploymentLocationOption?.ToString()!,
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
            IsRecruitVacancy = vacancy.IsRecruitVacancy,
            LongDescription = vacancy.LongDescription,
            NumberOfPositions = vacancy.NumberOfPositions,
            OutcomeDescription = vacancy.OutcomeDescription,
            PostedDate = vacancy.PostedDate,
            ProviderContactEmail = vacancy.ProviderContactEmail,
            ProviderContactName = vacancy.ProviderContactName,
            ProviderContactPhone = vacancy.ProviderContactPhone,
            ProviderName = vacancy.ProviderName,
            Qualifications = vacancy.Qualifications?.Select(q => (QualificationAzureSearchDocument)q).ToList() ?? [],
            Route = vacancy.Route,
            SearchTags = vacancy.SearchTags,
            Skills = vacancy.Skills?.ToList() ?? [],
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
    
    private static string FlattenAddress(Address address)
    {
        List<string> lines = [
            address.AddressLine1!,
            address.AddressLine2!,
            address.AddressLine3!,
            address.AddressLine4!,
            address.Postcode!,
        ];
        
        return string.Join(",", lines.Where(x => !string.IsNullOrWhiteSpace(x))).ToLowerInvariant();
    }
}