using System;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;
public class GetLiveVacancyApiRequest : IGetApiRequest
{
    private readonly Guid _vacancyId;

    public GetLiveVacancyApiRequest(Guid vacancyId)
    {
        _vacancyId = vacancyId;
    }

    public string GetUrl => $"livevacancy?vacancyId={_vacancyId}";
}
