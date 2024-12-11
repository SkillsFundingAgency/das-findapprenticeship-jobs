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
            string postcode)
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
            var result = RecruitIndexerJobHandler.SplitLiveVacanciesToMultipleByLocation(vacancies);

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
            string addressLine,
            string postcode,
            string addressLine1,
            string postcode1,
            string addressLine2,
            string postcode2)
        {
            // Arrange
            var vacancies = new List<LiveVacancy>
            {
                new()
                {
                    Address = new Address { AddressLine1 = addressLine, Postcode = postcode },
                    OtherAddresses =
                    [
                        new Address {AddressLine1 = addressLine1, Postcode = postcode1},
                        new Address {AddressLine1 = addressLine2, Postcode = postcode2}
                    ]
                }
            };

            // Act
            var result = RecruitIndexerJobHandler.SplitLiveVacanciesToMultipleByLocation(vacancies);

            // Assert
            result.Count().Should().Be(3);
        }

        [Test]
        public void SplitLiveVacanciesToMultipleByLocation_EmptyVacancies_ReturnsEmpty()
        {
            // Act
            var result = RecruitIndexerJobHandler.SplitLiveVacanciesToMultipleByLocation(
                new List<LiveVacancy>(0));

            // Assert
            result.Should().BeNullOrEmpty();
        }

        [Test, MoqAutoData]
        public void SplitLiveVacanciesToMultipleByLocation_MaintainsDataIntegrity(
            string vacancyId,
            string primaryAddressLine1,
            string primaryPostcode,
            string otherAddressLine1,
            string otherAddressPostcode)
        {
            // Arrange
            var vacancies = new List<LiveVacancy>
            {
                new()
                {
                    Id = vacancyId,
                    Address = new Address { AddressLine1 = primaryAddressLine1, Postcode = primaryPostcode },
                    OtherAddresses = [new Address {AddressLine1 = otherAddressLine1, Postcode = otherAddressPostcode}]
                }
            };

            // Act
            var result = RecruitIndexerJobHandler.SplitLiveVacanciesToMultipleByLocation(vacancies);

            // Assert
            var liveVacancies = result.ToList();

            liveVacancies.ToList().Count.Should().Be(2);
            var originalVacancy = liveVacancies.FirstOrDefault(r => r.Address.AddressLine1 == primaryAddressLine1);
            var newVacancy = liveVacancies.FirstOrDefault(r => r.Address.AddressLine1 == otherAddressLine1);

            originalVacancy.Should().NotBeNull();
            newVacancy.Should().NotBeNull();
            originalVacancy!.OtherAddresses.Count.Should().Be(1);
            newVacancy!.OtherAddresses.Count.Should().Be(1);
            newVacancy.Id.Should().Be($"{originalVacancy.Id}-1");
        }
    }
}
