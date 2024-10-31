using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Requests;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Api.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Handlers;

public class WhenHandlingVacancyClosingSoon
{
    [Test, MoqAutoData]
    public async Task Then_The_LiveVacancies_On_That_Closing_Day_Are_Retrieved(
        DateTime dateTime,
        List<LiveVacancy> liveVacancies,
        List<GetNhsLiveVacanciesApiResponse.NhsLiveVacancy> nhsLiveVacancies,
        [Frozen] Mock<IDateTimeService> dateTimeService,
        [Frozen] Mock<IFindApprenticeshipJobsService> recruitService,
        VacancyClosingSoonHandler sut)
    {
        dateTimeService.Setup(x => x.GetCurrentDateTime()).Returns(dateTime);
        var fixture = new Fixture();
        foreach (var liveVacancy in liveVacancies)
        {
            liveVacancy.VacancyReference = $"VAC{fixture.Create<long>()}";
        }
            
        var liveVacanciesApiResponse = new GetLiveVacanciesApiResponse
        {
            Vacancies = liveVacancies,
            PageNo = 1,
            PageSize = liveVacancies.Count,
            TotalLiveVacancies = liveVacancies.Count,
            TotalLiveVacanciesReturned = liveVacancies.Count,
            TotalPages = 1
        };

        recruitService.Setup(x => x.GetLiveVacancies(It.IsAny<int>(), It.IsAny<int>(), dateTime.AddDays(2))).ReturnsAsync(liveVacanciesApiResponse);
            

        var vacancies = await sut.Handle(2);

        using (new AssertionScope())
        {
            recruitService.Verify(x => x.GetLiveVacancies(It.IsAny<int>(), It.IsAny<int>(), dateTime.AddDays(2)), Times.Exactly(liveVacanciesApiResponse.TotalPages));
        }

        vacancies.Should().BeEquivalentTo(liveVacancies.Select(c => Convert.ToInt64(c.VacancyReference.Replace("VAC",""))).ToList());
    }

    [Test, MoqAutoData]
    public async Task Then_LiveVacancies_Is_Null_Empty_List_Returned(
        [Frozen] Mock<IFindApprenticeshipJobsService> recruitService,
        VacancyClosingSoonHandler sut)
    {
        recruitService.Setup(x => x.GetLiveVacancies(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>())).ReturnsAsync(() => null);
            
        var actual = await sut.Handle(5);

        actual.Should().BeEmpty();
    }
}