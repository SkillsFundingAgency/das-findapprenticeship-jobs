using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;
using System.Text.Json;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Extensions
{
    public static class LiveVacancyExtensions
    {
        // Define a small tolerance
        private const double Epsilon = 1e-10;

        public static IEnumerable<LiveVacancy> SplitLiveVacanciesToMultipleByLocation(IReadOnlyCollection<LiveVacancy> vacancies)
        {
            var multipleLocationVacancies = new List<LiveVacancy>();

            foreach (var liveVacancy in vacancies)
            {
                var counter = 1;
                foreach (var vacancyOtherAddress in liveVacancy.OtherAddresses)
                {
                    var vacancy = DeepCopyJson(liveVacancy);
                    if (vacancy == null) continue;

                    vacancy.Id = $"{liveVacancy.Id}-{counter}";
                    vacancy.IsPrimaryLocation = false;
                    vacancy.OtherAddresses.RemoveAll(r =>
                        r.AddressLine1 == vacancyOtherAddress.AddressLine1
                        && r.AddressLine2 == vacancyOtherAddress.AddressLine2
                        && r.AddressLine3 == vacancyOtherAddress.AddressLine3
                        && r.AddressLine4 == vacancyOtherAddress.AddressLine4
                        && r.Postcode == vacancyOtherAddress.Postcode
                        && Math.Abs(r.Longitude - vacancyOtherAddress.Longitude) < Epsilon
                        && Math.Abs(r.Latitude - vacancyOtherAddress.Latitude) < Epsilon);

                    if (liveVacancy.Address != null) vacancy.OtherAddresses.Add(liveVacancy.Address);
                    vacancy.Address = vacancyOtherAddress;
                    multipleLocationVacancies.Add(vacancy);
                    counter++;
                }
            }

            return vacancies
                .Concat(multipleLocationVacancies)
                .ToList();
        }

        private static T? DeepCopyJson<T>(T input)
        {
            var jsonString = JsonSerializer.Serialize(input);
            return JsonSerializer.Deserialize<T>(jsonString);
        }
    }
}
