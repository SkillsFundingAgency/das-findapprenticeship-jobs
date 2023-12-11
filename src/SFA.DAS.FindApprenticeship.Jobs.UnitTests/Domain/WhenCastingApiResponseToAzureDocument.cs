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
    public void Then_Fields_Are_Mapped()
    {
        var source = TestData.LiveVacancies.First();

        var apprenticeAzureSearchDocument = (ApprenticeAzureSearchDocument)source;

        using (new AssertionScope())
        {
            apprenticeAzureSearchDocument.Description.Should().BeEquivalentTo(source.Description);
            apprenticeAzureSearchDocument.Route.Should().BeEquivalentTo(source.Route);
            apprenticeAzureSearchDocument.EmployerName.Should().BeEquivalentTo(source.EmployerName);
            apprenticeAzureSearchDocument.HoursPerWeek.Should().Be((long)source.Wage.WeeklyHours);
            apprenticeAzureSearchDocument.ProviderName.Should().BeEquivalentTo(source.ProviderName);
            apprenticeAzureSearchDocument.StartDate.Should().Be(source.StartDate);
            apprenticeAzureSearchDocument.PostedDate.Should().Be(source.LiveDate);
            apprenticeAzureSearchDocument.ClosingDate.Should().Be(source.ClosingDate);
            apprenticeAzureSearchDocument.Title.Should().BeEquivalentTo(source.Title);
            apprenticeAzureSearchDocument.Ukprn.Should().Be(source.Ukprn);
            apprenticeAzureSearchDocument.VacancyReference.Should().Be($"VAC{source.VacancyReference}");
            AssertWageIsMapped(apprenticeAzureSearchDocument, source);
            AssertCourseIsMapped(apprenticeAzureSearchDocument, source);
            AssertAddressIsMapped(apprenticeAzureSearchDocument, source);
            apprenticeAzureSearchDocument.Location.Should().NotBeNull();
            apprenticeAzureSearchDocument.NumberOfPositions.Should().Be(source.NumberOfPositions);
        }
    }

    private static void AssertWageIsMapped(ApprenticeAzureSearchDocument apprenticeAzureSearchDocument, LiveVacancy source)
    {
        apprenticeAzureSearchDocument.Wage.WageAdditionalInformation.Should().BeEquivalentTo(source.Wage.WageAdditionalInformation);
        apprenticeAzureSearchDocument.Wage.WageAmount.Should().Be((long)source.Wage.FixedWageYearlyAmount);
        apprenticeAzureSearchDocument.Wage.WageType.Should().BeEquivalentTo(source.Wage.WageType);
        apprenticeAzureSearchDocument.Wage.WorkingWeekDescription.Should().BeEquivalentTo(source.Wage.WorkingWeekDescription);
    }

    private static void AssertCourseIsMapped(ApprenticeAzureSearchDocument apprenticeAzureSearchDocument, LiveVacancy source)
    {
        apprenticeAzureSearchDocument.Course.Level.Should().Be(source.Level);
        apprenticeAzureSearchDocument.Course.LarsCode.Should().Be(source.StandardLarsCode);
        apprenticeAzureSearchDocument.Course.Title.Should().BeEquivalentTo(source.ApprenticeshipTitle);
    }

    private static void AssertAddressIsMapped(ApprenticeAzureSearchDocument apprenticeAzureSearchDocument, LiveVacancy source)
    {
        apprenticeAzureSearchDocument.Address.AddressLine1.Should().BeEquivalentTo(source.Address.AddressLine1);
        apprenticeAzureSearchDocument.Address.AddressLine2.Should().BeEquivalentTo(source.Address.AddressLine2);
        apprenticeAzureSearchDocument.Address.AddressLine3.Should().BeEquivalentTo(source.Address.AddressLine3);
        apprenticeAzureSearchDocument.Address.AddressLine4.Should().BeEquivalentTo(source.Address.AddressLine4);
        apprenticeAzureSearchDocument.Address.Postcode.Should().BeEquivalentTo(source.Address.Postcode);
    }
}
