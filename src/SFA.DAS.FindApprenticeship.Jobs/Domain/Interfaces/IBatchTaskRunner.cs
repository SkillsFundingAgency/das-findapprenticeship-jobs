namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

public interface IBatchTaskRunner
{
    void AddTask(Func<Task> task);
    Task RunBatchesAsync();
}