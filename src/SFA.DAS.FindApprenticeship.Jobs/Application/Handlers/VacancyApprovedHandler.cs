﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
public class VacancyApprovedHandler : IVacancyApprovedHandler
{
    private readonly IAzureSearchHelper _azureSearchHelperService;
    private readonly IRecruitService _recruitService;

    public VacancyApprovedHandler(IAzureSearchHelper azureSearchHelper, IRecruitService recruitService)
    {
        _azureSearchHelperService = azureSearchHelper;
        _recruitService = recruitService;
    }

    public async Task Handle(VacancyApprovedEvent vacancyApprovedEvent, ILogger log)
    {
        log.LogInformation($"Vacancy Approved Event handler invoked at {DateTime.UtcNow}");

        //TODO: uncomment when FAI-1020 is done
        //var approvedVacancy = await _recruitService.GetLiveVacancy(vacancyApprovedEvent.VacancyReference);

        //var alias = await _azureSearchHelperService.GetAlias(Domain.Constants.AliasName);
        //var indexName = alias == null ? string.Empty : alias.Indexes.FirstOrDefault();

        //if (approvedVacancy != null && approvedVacancy.LiveVacancy != null && !string.IsNullOrEmpty(indexName))
        //{
        //    var document = (ApprenticeAzureSearchDocument)approvedVacancy.LiveVacancy;
        //    await _azureSearchHelperService.UploadDocuments(indexName, Enumerable.Empty<ApprenticeAzureSearchDocument>().Append(document));
        //}
        //else
        //{
        //    log.LogInformation($"Handle VacancyApprovedEvent failed with indexName {indexName} and vacancyId {vacancyApprovedEvent.VacancyId}");
        //}
    }
}
