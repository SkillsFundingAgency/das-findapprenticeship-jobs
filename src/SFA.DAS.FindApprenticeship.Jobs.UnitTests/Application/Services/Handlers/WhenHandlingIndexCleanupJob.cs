using AutoFixture.NUnit3;
using Azure.Search.Documents.Indexes.Models;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Application.Handlers;
using SFA.DAS.FindApprenticeship.Jobs.Domain;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Application.Services.Handlers
{
    [TestFixture]
    public class WhenHandlingIndexCleanupJob
    {
        [Test, MoqAutoData]
        public async Task Then_Indexes_Older_Than_6_Hours_Are_Deleted(
            [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
            [Frozen] Mock<IDateTimeService> dateTimeService,
            IndexCleanupJobHandler handler)
        {
            dateTimeService.Setup(x => x.GetCurrentDateTime()).Returns(DateTime.UtcNow);

            var indexes = new List<SearchIndex>
            {
                new SearchIndex(GetIndexName(DateTime.UtcNow.Subtract(new TimeSpan(6, 1, 0)))),
                new SearchIndex(GetIndexName(DateTime.UtcNow.Subtract(new TimeSpan(7, 0, 0)))),
                new SearchIndex(GetIndexName(DateTime.UtcNow.Subtract(new TimeSpan(48, 0, 0))))
            };

            azureSearchHelper.Setup(x => x.GetIndexes())
                .ReturnsAsync(() => indexes);

            await handler.Handle();

            azureSearchHelper.Verify(x => x.DeleteIndex(indexes[0].Name), Times.Once);
            azureSearchHelper.Verify(x => x.DeleteIndex(indexes[1].Name), Times.Once);
            azureSearchHelper.Verify(x => x.DeleteIndex(indexes[2].Name), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_Indexes_6_Hours_Old_Or_Less_Are_Retained(
            [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
            [Frozen] Mock<IDateTimeService> dateTimeService,
            IndexCleanupJobHandler handler)
        {
            var effectiveDate = DateTime.UtcNow;

            dateTimeService.Setup(x => x.GetCurrentDateTime()).Returns(effectiveDate);

            var indexes = new List<SearchIndex>
            {
                new SearchIndex(GetIndexName(effectiveDate)),
                new SearchIndex(GetIndexName(effectiveDate.Subtract(new TimeSpan(6, 0, 0))))
            };

            azureSearchHelper.Setup(x => x.GetIndexes())
                .ReturnsAsync(() => indexes);

            await handler.Handle();

            azureSearchHelper.Verify(x => x.DeleteIndex(It.IsAny<string>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_Indexes_Not_Conforming_To_Name_Convention_Are_Retained(
            [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
            [Frozen] Mock<IDateTimeService> dateTimeService,
            IndexCleanupJobHandler handler)
        {
            dateTimeService.Setup(x => x.GetCurrentDateTime()).Returns(DateTime.UtcNow);

            var indexes = new List<SearchIndex>
            {
                new SearchIndex("just_another_index"),
                new SearchIndex("apprenticeships_"),
                new SearchIndex("apprenticeships_index"),
            };

            azureSearchHelper.Setup(x => x.GetIndexes())
                .ReturnsAsync(() => indexes);
            
            await handler.Handle();

            azureSearchHelper.Verify(x => x.DeleteIndex(It.IsAny<string>()), Times.Never);
        }


        [Test, MoqAutoData]
        public async Task Then_The_Index_Currently_Aliased_Is_Retained_Irrespective_Of_Age(
            [Frozen] Mock<IAzureSearchHelper> azureSearchHelper,
            [Frozen] Mock<IDateTimeService> dateTimeService,
            IndexCleanupJobHandler handler)
        {
            var indexName = GetIndexName(DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)));

            dateTimeService.Setup(x => x.GetCurrentDateTime()).Returns(DateTime.UtcNow);

            azureSearchHelper.Setup(x => x.GetIndexes())
                .ReturnsAsync(() => new List<SearchIndex>
                {
                    new SearchIndex(indexName),
                });

            azureSearchHelper.Setup(x => x.GetAlias(Constants.AliasName))
                .ReturnsAsync(() => new SearchAlias(Constants.AliasName, new[] { indexName }));
            
            await handler.Handle();

            azureSearchHelper.Verify(x => x.DeleteIndex(It.IsAny<string>()), Times.Never);
        }

        private string GetIndexName(DateTime date)
        {
            return $"{Constants.IndexPrefix}{date.ToString(Constants.IndexDateSuffixFormat)}";
        }
    }
}
