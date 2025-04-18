﻿namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
public static class QueueNames
{
    public const string VacancyUpdated = "SFA.DAS.FindApprenticeshipJobs.VacancyUpdated";
    public const string TestHarness = "test-harness-queue";
    public const string VacancyClosed = "SFA.DAS.FindApprenticeshipJobs.VacancyClosed";
    public const string VacancyApproved = "SFA.DAS.FindApprenticeshipJobs.VacancyApproved";
}

public static class StorageQueueNames
{
    public const string VacancyClosing = "vacancy-closing";
    public const string SendSavedSearchNotificationAlert = "saved-search-notification";
    public const string GetSavedSearchNotifications = "get-saved-search-notification";
    public const string UpdateInactiveCandidateAccountsDormant = "update-inactive-accounts-dormant";
}
