using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Services;

public class BatchTaskRunner(ILogger<BatchTaskRunner> logger) : IBatchTaskRunner
{
    private readonly List<Func<Task>> _tasks = [];

    // Batch size
    private const int BatchSize = 100;
    
    // Method to add tasks to the list
    public void AddTask(Func<Task> task)
    {
        _tasks.Add(task);
    }

    // Method to execute tasks in batches
    public async Task RunBatchesAsync()
    {
        // Split tasks into batches of the specified size
        var taskBatches = _tasks
            .Select((task, index) => new { task, index })
            .GroupBy(x => x.index / BatchSize)
            .Select(g => g.Select(x => x.task).ToList());

        foreach (var batch in taskBatches)
        {
            // Run the current batch of tasks concurrently
            await Task.WhenAll(batch.Select(task => task()));
            logger.LogInformation("Batch completed");
        }
        
        logger.LogInformation("All batches completed");
    }
}