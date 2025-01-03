using SFA.DAS.FindApprenticeship.Jobs.Domain.Candidate;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers
{
    public class GetDormantCandidateAccountsHandler(
        IDateTimeService dateTimeService,
        IFindApprenticeshipJobsService findApprenticeshipJobsService) 
        : IGetDormantCandidateAccountsHandler
    {
        private const int PageSize = 1000;

        public async Task<List<Candidate>> Handle()
        {
            var pageNumber = 1;
            var totalPages = 100;

            var lastRunDateTime = dateTimeService.GetCurrentDateTime().AddMonths(-6);
            var batchDormantCandidates = new List<Candidate>();

            while (pageNumber <= totalPages)
            {
                var dormantCandidates = await findApprenticeshipJobsService.GetDormantCandidates(
                    lastRunDateTime.ToString("O"),
                    pageNumber,
                    PageSize);

                if (dormantCandidates is { Candidates.Count: > 0 })
                {
                    batchDormantCandidates.AddRange(dormantCandidates.Candidates.Select(x => (Candidate)x).ToList());

                    totalPages = dormantCandidates.TotalPages;

                    pageNumber++;
                }
                else
                {
                    break;
                }
            }

            return batchDormantCandidates;
        }
    }
}
