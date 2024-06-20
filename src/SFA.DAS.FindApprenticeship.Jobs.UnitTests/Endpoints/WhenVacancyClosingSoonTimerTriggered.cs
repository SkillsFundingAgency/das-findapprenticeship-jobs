using AutoFixture.NUnit3;
using Esfa.Recruit.Vacancies.Client.Domain.Events;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Endpoints;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Endpoints;

public class WhenVacancyClosingSoonTimerTriggered
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Will_Be_Handled_And_Items_Queued(
        ILogger log,
        VacancyClosedEvent command,
        long returnItem1,
        long returnItem2,
        [Frozen] Mock<ICollector<VacancyQueueItem>> collector,
        [Frozen] Mock<IVacancyClosingSoonHandler> handler,
        GetVacanciesClosingSoonTimerTrigger sut)
    {
        handler.Setup(x => x.Handle(2)).ReturnsAsync(new List<long>{returnItem1});
        handler.Setup(x => x.Handle(7)).ReturnsAsync(new List<long>{returnItem2});
        
        await sut.Run(It.IsAny<TimerInfo>(), log, collector.Object);

        collector.Verify(x=>x.Add(It.Is<VacancyQueueItem>(c=>c.VacancyReference==returnItem1 && c.DaysToExpire ==2)));
        collector.Verify(x=>x.Add(It.Is<VacancyQueueItem>(c=>c.VacancyReference==returnItem2 && c.DaysToExpire ==7)));
    }
}