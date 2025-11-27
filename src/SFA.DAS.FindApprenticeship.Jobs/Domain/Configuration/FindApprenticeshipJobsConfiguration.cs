namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration
{
    public class FindApprenticeshipJobsConfiguration
    {
        public string ApimBaseUrl { get; set; }
        public string ApimKey { get; set; }
        public string AzureSearchBaseUrl { get; set; }
        public string AzureSearchResource { get; set; }
        public string? ApimBaseUrlSecure { get; set; }
        public string? SecretClientUrl { get; set; }
        public string? SecretName { get; set; }
        public bool UseSecureGateway { get; set; }
    }
}
