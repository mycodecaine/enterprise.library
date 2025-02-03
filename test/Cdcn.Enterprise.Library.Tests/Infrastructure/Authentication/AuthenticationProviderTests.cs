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
using Cdcn.Enterprise.Library.Application.Core.Abstraction.Authentication.Contracts;
using Cdcn.Enterprise.Library.Infrastructure.Extension;
using Polly.Caching;

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
                TokenEndPoint = "https://example.com/token",
                BaseUrl = "https://example.com",
                RealmName = "test-realm",
            };
            _authenticationProvider = new AuthenticationProvider(_authenticationSetting, _httpClientFactoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task Login_ShouldReturnString_WhenCredentialsAreValid()
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

        [Test]
        public async Task GetAdminAccessToken_ShouldReturnString_WhenCredentialsAreValid()
        {
            // Arrange            
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
            var result = await _authenticationProvider.GetAdminAccessToken();

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
        public async Task GetAdminAccessToken_ShouldReturnFailure_WhenCredentialsAreInvalid()
        {
            // Arrange           
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
            var result = await _authenticationProvider.GetAdminAccessToken();

            // Assert
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual(AuthenticationErrors.InvalidUserNameOrPassword, result.Error);
        }

        [Test]
        public async Task LoginRefreshToken_ShouldReturnString_WhenCredentialsAreValid()
        {
            // Arrange           
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
            var result = await _authenticationProvider.Login(refreshToken);

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
        public async Task LoginRefreshToken_ShouldReturnFailure_WhenCredentialsAreInvalid()
        {
            // Arrange
            var refreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI1NTBlODQwMC1lMjliLTQxZDQtYTcxNi00NDY2NTU0NDAwMDAiLCJuYW1lIjoiSm9obiBEb2UiLCJpYXQiOjE1MTYyMzkwMjJ9.SidrxJjWYmR8cviBMT6wLny5TlabpHJ92FxQwMfJxeo";

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
            var result = await _authenticationProvider.Login(refreshToken);

            // Assert
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual(AuthenticationErrors.InvalidUserNameOrPassword, result.Error);
        }

        [Test]
        public async Task CreateUser_ShouldReturnSuccess_WhenUserIsCreated()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{}")
            };

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
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.AbsoluteUri == _authenticationSetting.TokenEndPoint),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);
            var httpClient = new HttpClient(httpClientMock.Object);
            _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var createUserUrl = $"{_authenticationSetting.BaseUrl}/admin/realms/{_authenticationSetting.RealmName}/users";

            // Mock the HTTP response for user creation
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            var user = new
            {
                username = "",
                email = "",
                firstName = "",
                lastName = "",
                enabled = true,
                credentials = new List<object>
                {
                    new
                    {
                        type = "password",
                        value = "",
                        temporary = false
                    }
                }
            };

            var userJson = Newtonsoft.Json.JsonConvert.SerializeObject(user);

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),                   
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponseMessage);

            var httpCreateClient = new HttpClient(mockHttpMessageHandler.Object);
            _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpCreateClient);

            // Act
            var result = await _authenticationProvider.CreateUser("testuser", "test@example.com", "Test", "User", "password");

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
        }

        



    }
}
