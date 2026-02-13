namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
public interface IRecruitIndexerJobHandler
{
    Task Handle(CancellationToken cancellationToken = default);
}
