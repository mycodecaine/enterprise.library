using Cdcn.Enterprise.Library.Infrastructure.Authentication.Setting;
using Cdcn.Enterprise.Library.Infrastructure.Authentication;
using Microsoft.Extensions.Logging;
using Moq.Protected;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Tests.Infrastructure.Authentication
{
    [TestFixture]
    public class AuthenticationProviderTests
    {
        private Mock<IHttpClientFactory> _httpClientFactoryMock;
        private Mock<ILogger<AuthenticationProvider>> _loggerMock;
        private AuthenticationSetting _authenticationSetting;
        private AuthenticationProvider _authenticationProvider;

        [SetUp]
        public void SetUp()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _loggerMock = new Mock<ILogger<AuthenticationProvider>>();
            _authenticationSetting = new AuthenticationSetting
            {
                ClientId = "test-client-id",
                ClientSecret = "test-client-secret",
                TokenEndPoint = "https://example.com/token"
            };
            _authenticationProvider = new AuthenticationProvider(_authenticationSetting, _httpClientFactoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task Login_ShouldReturnTokenResponse_WhenCredentialsAreValid()
        {
            // Arrange
            var username = "testuser";
            var password = "testpassword";
            var accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI1NTBlODQwMC1lMjliLTQxZDQtYTcxNi00NDY2NTU0NDAwMDAiLCJuYW1lIjoiSm9obiBEb2UiLCJpYXQiOjE1MTYyMzkwMjJ9.SidrxJjWYmR8cviBMT6wLny5TlabpHJ92FxQwMfJxeo";
            var refreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI1NTBlODQwMC1lMjliLTQxZDQtYTcxNi00NDY2NTU0NDAwMDAiLCJuYW1lIjoiSm9obiBEb2UiLCJpYXQiOjE1MTYyMzkwMjJ9.SidrxJjWYmR8cviBMT6wLny5TlabpHJ92FxQwMfJxeo";

            var expectedTokenResponse = new
            {
                access_token = accessToken,
                refresh_token = refreshToken,
                expires_in = 3600,
                refresh_expires_in = 7200
            };
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(expectedTokenResponse))
            };
            var httpClientMock = new Mock<HttpMessageHandler>();
            httpClientMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);
            var httpClient = new HttpClient(httpClientMock.Object);
            _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _authenticationProvider.Login(username, password);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            var actualResponse = JObject.Parse(result.Value);
            Assert.AreEqual(actualResponse["expires_in"].ToString(), "3600");
            Assert.AreEqual(actualResponse["refresh_expires_in"].ToString(), "7200");
            Assert.AreEqual(actualResponse["access_token"].ToString(), accessToken);
            Assert.AreEqual(actualResponse["refresh_token"].ToString(), refreshToken);
            
        }

        [Test]
        public async Task Login_ShouldReturnFailure_WhenCredentialsAreInvalid()
        {
            // Arrange
            var username = "invaliduser";
            var password = "invalidpassword";
            var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            var httpClientMock = new Mock<HttpMessageHandler>();
            httpClientMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);
            var httpClient = new HttpClient(httpClientMock.Object);
            _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _authenticationProvider.Login(username, password);

            // Assert
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual(AuthenticationErrors.InvalidUserNameOrPassword, result.Error);
        }
    }
}
