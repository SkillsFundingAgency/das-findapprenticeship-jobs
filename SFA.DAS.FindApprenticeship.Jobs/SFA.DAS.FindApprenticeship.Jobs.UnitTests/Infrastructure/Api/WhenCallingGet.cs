using AutoFixture.NUnit3;
using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeship.Jobs.UnitTests.Infrastructure.Api;
public class WhenCallingGet
{
    [Test, MoqAutoData]
    public async Task Then_The_Endpoint_Is_Called_With_Authentication_Header_And_Data_Returned(
        List<string> testObject,
        FindApprenticeshipJobsApiConfiguration config)
    {
        config.BaseUrl = $"https://test.local/{config.BaseUrl}/";
        var configMock = new Mock<IOptions<FindApprenticeshipJobsApiConfiguration>>();
        configMock.Setup(x => x.Value).Returns(config);
        var getTestRequest = new GetTestRequest();

        var response = new HttpResponseMessage()
        {
            Content = new StringContent(JsonConvert.SerializeObject(testObject)),
            StatusCode = System.Net.HttpStatusCode.Accepted
        };

        var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, config.BaseUrl + getTestRequest.GetUrl, config.Key, HttpMethod.Get);
        var client = new HttpClient(httpMessageHandler.Object);
        var apiClient = new ApiClient(client, configMock.Object);

        var actual = await apiClient.Get<List<string>>(getTestRequest);

        actual.Should().BeEquivalentTo(testObject);
    }

    [Test, AutoData]
    public void Then_If_It_Is_Not_Successful_An_Exception_Is_Thrown(
    FindApprenticeshipJobsApiConfiguration config)
    {
        config.BaseUrl = $"https://test.local/{config.BaseUrl}/";
        var configMock = new Mock<IOptions<FindApprenticeshipJobsApiConfiguration>>();
        configMock.Setup(x => x.Value).Returns(config);
        var getTestRequest = new GetTestRequest();
        var response = new HttpResponseMessage
        {
            Content = new StringContent(""),
            StatusCode = HttpStatusCode.BadRequest
        };

        var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, config.BaseUrl + getTestRequest.GetUrl, config.Key, HttpMethod.Get);
        var client = new HttpClient(httpMessageHandler.Object);
        var apiClient = new ApiClient(client, configMock.Object);

        Assert.ThrowsAsync<HttpRequestException>(() => apiClient.Get<List<string>>(getTestRequest));
    }

    [Test, AutoData]
    public async Task Then_If_It_Is_Not_Found_Default_Is_Returned(
    FindApprenticeshipJobsApiConfiguration config)
    {
        config.BaseUrl = $"https://test.local/{config.BaseUrl}/";
        var configMock = new Mock<IOptions<FindApprenticeshipJobsApiConfiguration>>();
        configMock.Setup(x => x.Value).Returns(config);
        var getTestRequest = new GetTestRequest();
        var response = new HttpResponseMessage
        {
            Content = new StringContent(""),
            StatusCode = HttpStatusCode.NotFound
        };

        var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, config.BaseUrl + getTestRequest.GetUrl, config.Key, HttpMethod.Get);
        var client = new HttpClient(httpMessageHandler.Object);
        var apiClient = new ApiClient(client, configMock.Object);

        var actual = await apiClient.Get<List<string>>(getTestRequest);

        actual.Should().BeNull();
    }

    private class GetTestRequest : IGetApiRequest
    {
        public GetTestRequest()
        {
        }
        public string GetUrl => $"test-url/get";
    }

}
