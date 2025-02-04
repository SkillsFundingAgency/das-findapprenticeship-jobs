using FluentAssertions.Execution;
using SFA.DAS.FindApprenticeship.Jobs.Application;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application;

public class ApprenticeAzureSearchDocumentFactoryTests
{
    [Test, MoqAutoData]
    public void Create_Maps_Deprecated_Address_Style_Vacancy(LiveVacancy liveVacancy)
    {
        // arrange
        liveVacancy.EmploymentLocationOption = null;
        liveVacancy.EmploymentLocations = null;
        liveVacancy.EmploymentLocationInformation = null;

        // act
        var documents = ApprenticeAzureSearchDocumentFactory.Create(liveVacancy).ToList();

        // assert
        documents.Should().HaveCount(1);
        var document = documents.Single();
        AssertDocumentIsMappedWithoutAddresses(document, liveVacancy);
        document.Id.Should().Be(liveVacancy.Id);
        document.Address.Should().BeEquivalentTo(liveVacancy.Address);
        document.Location.Should().BeEquivalentTo(new { liveVacancy.Address.Latitude, liveVacancy.Address.Longitude });
        document.AvailableWhere.Should().BeNull();
    }
    
    [Test, MoqAutoData]
    public void Create_Maps_OneLocation_Vacancy(LiveVacancy liveVacancy)
    {
        // arrange
        var address = liveVacancy.EmploymentLocations!.First();
        liveVacancy.EmploymentLocationOption = AvailableWhere.OneLocation;
        liveVacancy.EmploymentLocations = [address];
        liveVacancy.EmploymentLocationInformation = null;
        liveVacancy.Address = null;

        // act
        var documents = ApprenticeAzureSearchDocumentFactory.Create(liveVacancy).ToList();

        // assert
        documents.Should().HaveCount(1);
        var document = documents.Single();
        AssertDocumentIsMappedWithoutAddresses(document, liveVacancy);
        document.Id.Should().Be(liveVacancy.Id);
        document.Address.Should().BeEquivalentTo(address);
        document.Location.Should().BeEquivalentTo(new { address.Latitude, address.Longitude });
        document.AvailableWhere.Should().Be(nameof(AvailableWhere.OneLocation));
    }
    
    [Test, MoqAutoData]
    public void Create_Maps_RecruitNationally_Vacancy(LiveVacancy liveVacancy)
    {
        // arrange
        liveVacancy.EmploymentLocationOption = AvailableWhere.AcrossEngland;
        liveVacancy.EmploymentLocations = null;
        liveVacancy.Address = null;

        // act
        var documents = ApprenticeAzureSearchDocumentFactory.Create(liveVacancy).ToList();

        // assert
        documents.Should().HaveCount(1);
        var document = documents.Single();
        AssertDocumentIsMappedWithoutAddresses(document, liveVacancy);
        document.Id.Should().Be(liveVacancy.Id);
        document.Address.Should().BeNull();
        document.Location.Should().BeNull();
        document.EmploymentLocationInformation.Should().Be(liveVacancy.EmploymentLocationInformation);
        document.AvailableWhere.Should().Be(nameof(AvailableWhere.AcrossEngland));
    }
    
    [Test, MoqAutoData]
    public void Create_Maps_MultipleLocations_Vacancy(LiveVacancy liveVacancy)
    {
        // arrange
        liveVacancy.EmploymentLocationOption = AvailableWhere.MultipleLocations;
        liveVacancy.Address = null;
        liveVacancy.EmploymentLocationInformation = null;

        // act
        var documents = ApprenticeAzureSearchDocumentFactory.Create(liveVacancy).ToList();

        // assert
        documents.Should().HaveCount(liveVacancy.EmploymentLocations!.Count);
        documents.Should().AllSatisfy(document =>
        {
            AssertDocumentIsMappedWithoutAddresses(document, liveVacancy);
            liveVacancy.EmploymentLocations.Should().ContainEquivalentOf(document.Address);
            document.OtherAddresses.Should().NotBeNull();
            document.OtherAddresses.Should().HaveCount(liveVacancy.EmploymentLocations!.Count - 1);
            document.OtherAddresses.Should().NotContainEquivalentOf(document.Address);
            document.Location.Should().BeEquivalentTo(new { document.Address!.Latitude, document.Address.Longitude });
            document.EmploymentLocationInformation.Should().BeNull();
            document.AvailableWhere.Should().Be(nameof(AvailableWhere.MultipleLocations));
        });
    }
    
    [Test, MoqAutoData]
    public void Create_Maps_MultipleLocations_Vacancy_With_Unique_Ids(LiveVacancy liveVacancy)
    {
        // arrange
        liveVacancy.EmploymentLocationOption = AvailableWhere.MultipleLocations;
        liveVacancy.Address = null;
        liveVacancy.EmploymentLocationInformation = null;

        // act
        var documents = ApprenticeAzureSearchDocumentFactory.Create(liveVacancy).ToList();

        // assert
        documents.Should().HaveCountGreaterThan(1);
        documents.Select(x => x.Id).Distinct().Count().Should().Be(documents.Count);
        documents.First().Id.Should().Be(liveVacancy.Id);
        documents.First().AvailableWhere.Should().Be(nameof(AvailableWhere.MultipleLocations));
    }

    private static void AssertDocumentIsMappedWithoutAddresses(ApprenticeAzureSearchDocument document, LiveVacancy source)
    {
        using (new AssertionScope())
        {
            document.ApplicationUrl.Should().Be(source.ApplicationUrl);
            document.ApplicationInstructions.Should().Be(source.ApplicationInstructions);
            document.AccountPublicHashedId.Should().Be(source.AccountPublicHashedId);
            document.AccountLegalEntityPublicHashedId.Should().Be(source.AccountLegalEntityPublicHashedId);
            document.AdditionalQuestion1.Should().Be(source.AdditionalQuestion1);
            document.AdditionalQuestion2.Should().Be(source.AdditionalQuestion2);
            document.AnonymousEmployerName.Should().Be(source.AnonymousEmployerName);
            document.ApplicationMethod.Should().Be(source.ApplicationMethod);
            document.ApprenticeshipLevel.Should().Be(source.ApprenticeshipLevel);
            document.ClosingDate.Should().Be(source.ClosingDate);
            document.Course.Level.Should().Be(source.Level.ToString());
            document.Course.LarsCode.Should().Be(source.StandardLarsCode);
            document.Course.Title.Should().BeEquivalentTo(source.ApprenticeshipTitle);
            document.Course.RouteCode.Should().Be(source.RouteCode);
            document.Description.Should().BeEquivalentTo(source.Description);
            document.EmployerContactEmail.Should().Be(source.EmployerContactEmail);
            document.EmployerContactPhone.Should().Be(source.EmployerContactPhone);
            document.EmployerName.Should().BeEquivalentTo(source.EmployerName);
            document.EmployerWebsiteUrl.Should().Be(source.EmployerWebsiteUrl);
            document.EmployerContactName.Should().Be(source.EmployerContactName);
            document.HoursPerWeek.Should().Be((double)source.Wage.WeeklyHours);
            document.IsDisabilityConfident.Should().Be(source.IsDisabilityConfident);
            document.IsEmployerAnonymous.Should().Be(source.IsEmployerAnonymous);
            document.IsPositiveAboutDisability.Should().Be(source.IsPositiveAboutDisability);
            document.IsPrimaryLocation.Should().Be(source.IsPrimaryLocation);
            document.IsRecruitVacancy.Should().Be(source.IsRecruitVacancy);
            document.LongDescription.Should().Be(source.LongDescription);
            document.NumberOfPositions.Should().Be(source.NumberOfPositions);
            document.OutcomeDescription.Should().Be(source.OutcomeDescription);
            document.ProviderName.Should().BeEquivalentTo(source.ProviderName);
            document.Qualifications.Should().BeEquivalentTo(source.Qualifications, opt => opt.Excluding(x => x.Weighting));
            document.Route.Should().BeEquivalentTo(source.Route);
            document.PostedDate.Should().Be(source.PostedDate);
            document.Skills.Should().BeEquivalentTo(source.Skills);
            document.StartDate.Should().Be(source.StartDate);
            document.ThingsToConsider.Should().Be(source.ThingsToConsider);
            document.Title.Should().BeEquivalentTo(source.Title);
            document.TrainingDescription.Should().Be(source.TrainingDescription);
            document.TypicalJobTitles.Should().BeEquivalentTo(source.TypicalJobTitles);
            document.Ukprn.Should().Be(source.Ukprn.ToString());
            document.VacancyLocationType.Should().Be(source.VacancyLocationType);
            document.VacancyReference.Should().Be($"VAC{source.VacancyReference}");
            document.Wage.Should().NotBeNull();
            document.Wage?.WageAdditionalInformation.Should().BeEquivalentTo(source.Wage.WageAdditionalInformation);
            document.Wage?.WageAmount.Should().Be((long)source.Wage.FixedWageYearlyAmount);
            document.Wage?.WageType.Should().BeEquivalentTo(source.Wage.WageType);
            document.Wage?.WorkingWeekDescription.Should().BeEquivalentTo(source.Wage.WorkingWeekDescription);
            document.Wage?.Duration.Should().Be(source.Wage.Duration);
            document.WageText.Should().Be(source.Wage.WageText);
            document.EmploymentLocationInformation.Should().Be(source.EmploymentLocationInformation);
        }
    }
}