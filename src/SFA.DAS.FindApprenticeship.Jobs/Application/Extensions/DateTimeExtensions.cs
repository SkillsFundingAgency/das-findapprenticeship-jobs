namespace SFA.DAS.FindApprenticeship.Jobs.Application.Extensions
{
    public static class TimeSpanExtensions
    {
        public static TimeSpan RemoveSeconds(this TimeSpan source)
        {
            return new TimeSpan(source.Days, source.Hours, source.Minutes, 0);
        }
    }
}
