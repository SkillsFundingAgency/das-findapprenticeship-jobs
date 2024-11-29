using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Handlers;

public class WhenHandlingSendApplicationReminder
{
    [Test, MoqAutoData]
    public async Task Then_The_Reminder_Is_Sent(
        long vacancyRef,
        int daysUntilExpiry,
        [Frozen] Mock<IFindApprenticeshipJobsService> findApprenticeshipJobsService,
        SendApplicationReminderHandler handler)
    {
        await handler.Handle(vacancyRef, daysUntilExpiry);

        findApprenticeshipJobsService.Verify(
            x => x.SendApplicationClosingSoonReminder(vacancyRef, daysUntilExpiry));
    }
}