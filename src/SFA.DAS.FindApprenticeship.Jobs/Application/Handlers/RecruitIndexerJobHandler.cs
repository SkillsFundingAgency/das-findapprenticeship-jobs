﻿using SFA.DAS.FindApprenticeship.Jobs.Domain;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
public class RecruitIndexerJobHandler(
    IFindApprenticeshipJobsService findApprenticeshipJobsService,
    IAzureSearchHelper azureSearchHelperService,
    IDateTimeService dateTimeService)
    : IRecruitIndexerJobHandler
{
    private const int PageSize = 500;

    public async Task Handle()
    {
        var indexName = $"{Constants.IndexPrefix}{dateTimeService.GetCurrentDateTime().ToString(Constants.IndexDateSuffixFormat)}";

        await azureSearchHelperService.CreateIndex(indexName);

        var pageNo = 1;
        var totalPages = 100;
        var updateAlias = false;
        var batchDocuments = new List<ApprenticeAzureSearchDocument>();

        while (pageNo <= totalPages)
        {
            var liveVacancies = await findApprenticeshipJobsService.GetLiveVacancies(pageNo, PageSize);
            var nhsLiveVacancies = await findApprenticeshipJobsService.GetNhsLiveVacancies();

            totalPages = Math.Max(liveVacancies?.TotalPages ?? 0, nhsLiveVacancies?.TotalPages ?? 0);

            if (liveVacancies != null || nhsLiveVacancies != null)
            {
                if (liveVacancies != null && liveVacancies.Vacancies.Any())
                {
                    var vacanciesWithOneLocation = liveVacancies.Vacancies.Where(fil => fil.OtherAddresses.Count == 0)
                        .Select(a => (ApprenticeAzureSearchDocument) a)
                        .ToList();

                    var vacanciesWithMultipleLocations = SplitLiveVacanciesToMultipleByLocation(liveVacancies
                        .Vacancies
                        .Where(fil => fil.OtherAddresses.Count > 0).ToList());

                    batchDocuments.AddRange(vacanciesWithOneLocation.Concat(vacanciesWithMultipleLocations.Select(a => (ApprenticeAzureSearchDocument)a)).ToList());
                }

                if (nhsLiveVacancies != null && nhsLiveVacancies.Vacancies.Any())
                {
                    batchDocuments.AddRange(nhsLiveVacancies.Vacancies.Select(vacancy => (ApprenticeAzureSearchDocument) vacancy));
                }

                await azureSearchHelperService.UploadDocuments(indexName, batchDocuments);
                pageNo++;
                updateAlias = true;
            }
            else
            {
                break;
            }
        }

        if (updateAlias)
        {
            await azureSearchHelperService.UpdateAlias(Constants.AliasName, indexName);
        }
    }

    public IEnumerable<LiveVacancy> SplitLiveVacanciesToMultipleByLocation(IReadOnlyCollection<LiveVacancy> vacancies)
    {
        var multipleLocationVacancies = new List<LiveVacancy>();

        foreach (var liveVacancy in vacancies)
        {
            foreach (var vacancyOtherAddress in liveVacancy.OtherAddresses)
            {
                var vacancy = DeepCopyJson(liveVacancy);
                if (vacancy == null) continue;

                vacancy.IsPrimaryLocation = false;
                vacancy.OtherAddresses.RemoveAll(r =>
                    r.AddressLine1 == vacancyOtherAddress.AddressLine1
                    && r.AddressLine2 == vacancyOtherAddress.AddressLine2
                    && r.AddressLine3 == vacancyOtherAddress.AddressLine3
                    && r.AddressLine4 == vacancyOtherAddress.AddressLine4
                    && r.Postcode == vacancyOtherAddress.Postcode
                    && Math.Abs(r.Longitude - vacancyOtherAddress.Longitude) == 0
                    && Math.Abs(r.Latitude - vacancyOtherAddress.Latitude) == 0);

                if (liveVacancy.Address != null) vacancy.OtherAddresses.Add(liveVacancy.Address);
                vacancy.Address = vacancyOtherAddress;
                multipleLocationVacancies.Add(vacancy);
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