using SFA.DAS.FindApprenticeship.Jobs.Domain;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Candidate;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Handlers
{
    public class UpdateCandidateStatusHandler(
        ILogger<UpdateCandidateStatusHandler> logger,
        IFindApprenticeshipJobsService findApprenticeshipJobsService,
        IBatchTaskRunner batchTaskRunner) : IUpdateCandidateStatusHandler
    {
        public async Task BatchHandle(List<Candidate> candidates)
        {
            // Add tasks to the runner
            for (var index = 0; index < candidates.Count; index++)
            {
                var taskId = index + 1;
                var candidate = candidates[index];
                batchTaskRunner.AddTask(async () =>
                {
                    logger.LogInformation("Update Candidate Status Task {TaskId} started", taskId);
                    await findApprenticeshipJobsService.UpdateCandidateStatus(
                        candidate.GovUkIdentifier,
                        candidate.Email,
                        CandidateStatus.Dormant);
                    logger.LogInformation("Update Candidate Status Task {TaskId} completed", taskId);
                });
            }

            // Run tasks in batches
            await batchTaskRunner.RunBatchesAsync();
        }
    }
}