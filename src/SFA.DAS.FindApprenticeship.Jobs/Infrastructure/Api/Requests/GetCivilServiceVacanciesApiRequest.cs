using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;
public record GetCivilServiceVacanciesApiRequest : IGetApiRequest
{
    public string GetUrl => "CivilServiceVacancies";
}