﻿using Cdcn.Enterprise.Library.Application.Core.Services;
using Cdcn.Enterprise.Library.Infrastructure.Authentication;
using Cdcn.Enterprise.Library.Infrastructure.Authentication.Setting;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Cdcn.Enterprise.Library.Tests.Infrastructure.Authentication
{
    [TestFixture]
    public class AuthenticationProviderTests
    {
        private Mock<IHttpService> _httpServiceMock;
        private Mock<ILogger<AuthenticationProvider>> _loggerMock;
        private AuthenticationSetting _authenticationSetting;
        private AuthenticationProvider _authenticationProvider;

        [SetUp]
        public void SetUp()
        {
            _httpServiceMock = new Mock<IHttpService>();
            _loggerMock = new Mock<ILogger<AuthenticationProvider>>();
            _authenticationSetting = new AuthenticationSetting
            {
                ClientId = "test-client-id",
                ClientSecret = "test-client-secret",
                TokenEndPoint = "https://example.com/token",
                BaseUrl = "https://example.com",
                RealmName = "test-realm",
            };
            _authenticationProvider = new AuthenticationProvider(_authenticationSetting, _httpServiceMock.Object, _loggerMock.Object);
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
            _httpServiceMock.Setup(service => service.PostAsync(It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>(), It.IsAny<string>())).ReturnsAsync(responseMessage);          

           
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
            _httpServiceMock.Setup(service => service.PostAsync(It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>(), It.IsAny<string>())).ReturnsAsync(responseMessage);


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
            _httpServiceMock.Setup(service => service.PostAsync(It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>(), It.IsAny<string>())).ReturnsAsync(responseMessage);


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
            _httpServiceMock.Setup(service => service.PostAsync(It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>(), It.IsAny<string>())).ReturnsAsync(responseMessage);


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
            _httpServiceMock.Setup(service => service.PostAsync(It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>(), It.IsAny<string>())).ReturnsAsync(responseMessage);


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
            _httpServiceMock.Setup(service => service.PostAsync(It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>(), It.IsAny<string>())).ReturnsAsync(responseMessage);

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
            _httpServiceMock.Setup(service => service.PostAsync(It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>(), It.IsAny<string>())).ReturnsAsync(responseMessage);


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

            _httpServiceMock.Setup(service => service.PostJsonAsync(It.IsAny<string>(),It.IsAny<Object>(), It.IsAny<string>())).ReturnsAsync(responseMessage);


            // Act
            var result = await _authenticationProvider.CreateUser("testuser", "test@example.com", "Test", "User", "password");

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
        }

        



    }
}
