﻿namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Services.DateTimeService
{
    [TestFixture]
    public class WhenGettingCurrentDateTime
    {
        [Test, MoqAutoData]
        public void Then_The_Expected_Result_Is_Returned(Jobs.Application.Services.DateTimeService sut)
        {
            var result = sut.GetCurrentDateTime();
            result.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10));
        }
    }
}
