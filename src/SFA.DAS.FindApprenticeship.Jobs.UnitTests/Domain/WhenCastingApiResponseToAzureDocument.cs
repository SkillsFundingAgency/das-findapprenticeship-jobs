using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Domain;
public class WhenCastingApiResponseToAzureDocument
{
    [Test, MoqAutoData]
    public void Then_Fields_Are_Mapped(LiveVacancy source)
    {
        var apprenticeAzureSearchDocument = (ApprenticeAzureSearchDocument)source;

        using (new AssertionScope())
        {
            apprenticeAzureSearchDocument.Description.Should().BeEquivalentTo(source.Description);
            apprenticeAzureSearchDocument.Route.Should().BeEquivalentTo(source.Route);
            apprenticeAzureSearchDocument.EmployerName.Should().BeEquivalentTo(source.EmployerName);
            apprenticeAzureSearchDocument.ApprenticeshipLevel.Should().Be(source.ApprenticeshipLevel);
            apprenticeAzureSearchDocument.ApplicationMethod.Should().Be(source.ApplicationMethod);
            apprenticeAzureSearchDocument.ApplicationUrl.Should().Be(source.ApplicationUrl);
            apprenticeAzureSearchDocument.AccountPublicHashedId.Should().Be(source.AccountPublicHashedId);
            apprenticeAzureSearchDocument.AccountLegalEntityPublicHashedId.Should().Be(source.AccountLegalEntityPublicHashedId);
            apprenticeAzureSearchDocument.HoursPerWeek.Should().Be((long)source.Wage.WeeklyHours);
            apprenticeAzureSearchDocument.ProviderName.Should().BeEquivalentTo(source.ProviderName);
            apprenticeAzureSearchDocument.StartDate.Should().Be(source.StartDate);
            apprenticeAzureSearchDocument.PostedDate.Should().Be(source.PostedDate);
            apprenticeAzureSearchDocument.ClosingDate.Should().Be(source.ClosingDate);
            apprenticeAzureSearchDocument.Title.Should().BeEquivalentTo(source.Title);
            apprenticeAzureSearchDocument.Ukprn.Should().Be(source.Ukprn.ToString());
            apprenticeAzureSearchDocument.VacancyReference.Should().Be($"VAC{source.VacancyReference}");
            apprenticeAzureSearchDocument.WageText.Should().Be(source.Wage.WageText);
            AssertWageIsMapped(apprenticeAzureSearchDocument, source);
            AssertCourseIsMapped(apprenticeAzureSearchDocument, source);
            AssertAddressIsMapped(apprenticeAzureSearchDocument, source);
            apprenticeAzureSearchDocument.Location.Should().NotBeNull();
            apprenticeAzureSearchDocument.NumberOfPositions.Should().Be(source.NumberOfPositions);
            apprenticeAzureSearchDocument.LongDescription.Should().Be(source.LongDescription);
            apprenticeAzureSearchDocument.OutcomeDescription.Should().Be(source.OutcomeDescription);
            apprenticeAzureSearchDocument.TrainingDescription.Should().Be(source.TrainingDescription);
            apprenticeAzureSearchDocument.Skills.Should().BeEquivalentTo(source.Skills);
            apprenticeAzureSearchDocument.ThingsToConsider.Should().Be(source.ThingsToConsider);
            apprenticeAzureSearchDocument.Id.Should().Be(source.Id);
            apprenticeAzureSearchDocument.AnonymousEmployerName.Should().Be(source.AnonymousEmployerName);
            apprenticeAzureSearchDocument.IsDisabilityConfident.Should().Be(source.IsDisabilityConfident);
            apprenticeAzureSearchDocument.IsPositiveAboutDisability.Should().Be(source.IsPositiveAboutDisability);
            apprenticeAzureSearchDocument.IsEmployerAnonymous.Should().Be(source.IsEmployerAnonymous);
            apprenticeAzureSearchDocument.IsRecruitVacancy.Should().Be(source.IsRecruitVacancy);
            apprenticeAzureSearchDocument.VacancyLocationType.Should().Be(source.VacancyLocationType);
            apprenticeAzureSearchDocument.EmployerWebsiteUrl.Should().Be(source.EmployerWebsiteUrl);
            apprenticeAzureSearchDocument.EmployerContactPhone.Should().Be(source.EmployerContactPhone);
            apprenticeAzureSearchDocument.EmployerContactEmail.Should().Be(source.EmployerContactEmail);
            apprenticeAzureSearchDocument.EmployerContactName.Should().Be(source.EmployerContactName);
            AssetQualificationsAreMapped(apprenticeAzureSearchDocument, source);
        }
    }

    private static void AssertWageIsMapped(ApprenticeAzureSearchDocument apprenticeAzureSearchDocument, LiveVacancy source)
    {
        apprenticeAzureSearchDocument.Wage.WageAdditionalInformation.Should().BeEquivalentTo(source.Wage.WageAdditionalInformation);
        apprenticeAzureSearchDocument.Wage.WageAmount.Should().Be((long)source.Wage.FixedWageYearlyAmount);
        apprenticeAzureSearchDocument.Wage.WageType.Should().BeEquivalentTo(source.Wage.WageType);
        apprenticeAzureSearchDocument.Wage.WorkingWeekDescription.Should().BeEquivalentTo(source.Wage.WorkingWeekDescription);
        apprenticeAzureSearchDocument.Wage.Duration.Should().Be(source.Wage.Duration);
    }

    private static void AssertCourseIsMapped(ApprenticeAzureSearchDocument apprenticeAzureSearchDocument, LiveVacancy source)
    {
        apprenticeAzureSearchDocument.Course.Level.Should().Be(source.Level);
        apprenticeAzureSearchDocument.Course.LarsCode.Should().Be(source.StandardLarsCode);
        apprenticeAzureSearchDocument.Course.Title.Should().BeEquivalentTo(source.ApprenticeshipTitle);
        apprenticeAzureSearchDocument.Course.RouteCode.Should().Be(source.RouteCode);
    }

    private static void AssertAddressIsMapped(ApprenticeAzureSearchDocument apprenticeAzureSearchDocument, LiveVacancy source)
    {
        apprenticeAzureSearchDocument.Address.AddressLine1.Should().BeEquivalentTo(source.Address.AddressLine1);
        apprenticeAzureSearchDocument.Address.AddressLine2.Should().BeEquivalentTo(source.Address.AddressLine2);
        apprenticeAzureSearchDocument.Address.AddressLine3.Should().BeEquivalentTo(source.Address.AddressLine3);
        apprenticeAzureSearchDocument.Address.AddressLine4.Should().BeEquivalentTo(source.Address.AddressLine4);
        apprenticeAzureSearchDocument.Address.Postcode.Should().BeEquivalentTo(source.Address.Postcode);
        apprenticeAzureSearchDocument.Address.Latitude.Should().Be(source.Address.Latitude);
        apprenticeAzureSearchDocument.Address.Longitude.Should().Be(source.Address.Longitude);

    }

    private static void AssetQualificationsAreMapped(ApprenticeAzureSearchDocument apprenticeAzureSearchDocument, LiveVacancy source)
    {
        apprenticeAzureSearchDocument.Qualifications.Should().BeEquivalentTo(source.Qualifications,
            opt =>
                opt.Excluding(x => x.Weighting));
    }
}
