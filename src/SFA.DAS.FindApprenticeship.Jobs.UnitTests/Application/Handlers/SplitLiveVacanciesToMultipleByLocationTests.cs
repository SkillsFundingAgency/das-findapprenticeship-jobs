using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Handlers
{
    [TestFixture]
    public class SplitLiveVacanciesToMultipleByLocationTests
    {
        [Test, MoqAutoData]
        public void SplitLiveVacanciesToMultipleByLocation_SingleLocation_ReturnsCorrectResults(
            string addressLine1,
            string postcode,
            RecruitIndexerJobHandler sut)
        {
            // Arrange
            var vacancies = new List<LiveVacancy>
            {
                new()
                {
                    Address = new Address { AddressLine1 = addressLine1, Postcode = postcode },
                    OtherAddresses = []
                }
            };

            // Act
            var result = sut.SplitLiveVacanciesToMultipleByLocation(vacancies);

            // Assert
            var liveVacancies = result.ToList();

            liveVacancies.Should().NotBeNull();
            liveVacancies.Count.Should().Be(1);

            var document = liveVacancies.First();
            document.Address!.AddressLine1.Should().Be(addressLine1);
            document.Address.Postcode.Should().Be(postcode);
        }

        [Test, MoqAutoData]
        public void SplitLiveVacanciesToMultipleByLocation_MultipleLocations_SplitsCorrectly(
            RecruitIndexerJobHandler sut)
        {
            // Arrange
            var vacancies = new List<LiveVacancy>
            {
                new LiveVacancy
                {
                    Address = new Address { AddressLine1 = "123 Main St", Postcode = "12345" },
                    OtherAddresses =
                    [
                        new Address {AddressLine1 = "456 Elm St", Postcode = "67890"},
                        new Address {AddressLine1 = "789 Pine St", Postcode = "54321"}
                    ]
                }
            };

            // Act
            var result = sut.SplitLiveVacanciesToMultipleByLocation(vacancies);

            // Assert
            result.Count().Should().Be(3);
        }

        [Test, MoqAutoData]
        public void SplitLiveVacanciesToMultipleByLocation_EmptyVacancies_ReturnsEmpty(
            RecruitIndexerJobHandler sut)
        {
            // Act
            var result = sut.SplitLiveVacanciesToMultipleByLocation(
                new List<LiveVacancy>(0));

            // Assert
            result.Should().BeNullOrEmpty();
        }

        [Test, MoqAutoData]
        public void SplitLiveVacanciesToMultipleByLocation_MaintainsDataIntegrity(
            RecruitIndexerJobHandler sut)
        {
            // Arrange
            var vacancies = new List<LiveVacancy>
            {
                new LiveVacancy
                {
                    Address = new Address { AddressLine1 = "123 Main St", Postcode = "12345" },
                    OtherAddresses = [new Address {AddressLine1 = "456 Elm St", Postcode = "67890"}]
                }
            };

            // Act
            var result = sut.SplitLiveVacanciesToMultipleByLocation(vacancies);

            // Assert
            var liveVacancies = result.ToList();

            liveVacancies.ToList().Count.Should().Be(2);
            var originalVacancy = liveVacancies.FirstOrDefault(r => r.Address.AddressLine1 == "123 Main St");
            var newVacancy = liveVacancies.FirstOrDefault(r => r.Address.AddressLine1 == "456 Elm St");

            originalVacancy.Should().NotBeNull();
            newVacancy.Should().NotBeNull();
            originalVacancy!.OtherAddresses.Count.Should().Be(1);
            newVacancy!.OtherAddresses.Count.Should().Be(1);
        }
    }
}
