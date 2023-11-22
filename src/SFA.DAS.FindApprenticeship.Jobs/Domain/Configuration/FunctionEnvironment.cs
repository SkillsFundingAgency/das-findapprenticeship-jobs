namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration
{
    public class FunctionEnvironment
    {
        public virtual string EnvironmentName { get; }

        public FunctionEnvironment(string environmentName)
        {
            EnvironmentName = environmentName;
        }
    }
}
