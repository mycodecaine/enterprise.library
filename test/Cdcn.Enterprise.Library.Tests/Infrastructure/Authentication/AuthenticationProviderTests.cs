using Cdcn.Enterprise.Library.Application.Core.Abstraction.Caching;
using Cdcn.Enterprise.Library.Infrastructure.Authentication;
using Cdcn.Enterprise.Library.Infrastructure.Authentication.Setting;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Moq.Protected;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Cdcn.Enterprise.Library.Tests.Infrastructure.Authentication
{
    [TestFixture]
    public class AuthenticationProviderTests
    {
        private Mock<IHttpClientFactory> _mockHttpClientFactory;
        private Mock<ICachingService> _mockCachingService;
        private Mock<IMediator> _mockMediator;
        private Mock<IOptions<AuthenticationSetting>> _mockAuthenticationSetting;
        private AuthenticationProvider _authenticationProvider;

        [SetUp]
        public void SetUp()
        {
            // Mock AuthenticationSetting
            _mockAuthenticationSetting = new Mock<IOptions<AuthenticationSetting>>();
            _mockAuthenticationSetting.SetupGet(x => x.Value).Returns(new AuthenticationSetting
            {
                Admin = "adminUser",
                Password = "adminPassword",
                BaseUrl = "https://example.com",
                TokenEndPoint = "/token",
                ClientId = "clientId",
                ClientSecret = "clientSecret",
                RealmName = "realm"
            });

            // Mock HttpClientFactory
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();

            // Mock ICachingService
            _mockCachingService = new Mock<ICachingService>();

            // Mock IMediator
            _mockMediator = new Mock<IMediator>();

            // Create instance of AuthenticationProvider
            _authenticationProvider = new AuthenticationProvider(
                _mockAuthenticationSetting.Object,
                _mockHttpClientFactory.Object,
                _mockCachingService.Object,
                _mockMediator.Object
            );
        }

        public static string GenerateMockJwtToken(string userId)
        {
            // Define the secret key used to sign the token (this should match your actual key)
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key"));
            //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            // Add claims to the token, if any
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            // Add other claims here if needed
        };

            // Define the token
            var token = new JwtSecurityToken(
                issuer: "testIssuer",
                audience: "testAudience",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: signingCredentials);

            // Generate token string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [Test]
        public async Task Login_ShouldReturnSuccess_WhenValidCredentials()
        {
            var userId = Guid.NewGuid().ToString();
            var jwtToken = GenerateMockJwtToken(userId);
            // Mock HttpMessageHandler
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent($"{{\"access_token\": \"{jwtToken}\",\"refresh_token\": \"{{jwtToken}}\", \"expires_in\": 3600, \"refresh_expires_in\": 7200}}", Encoding.UTF8, "application/json")
                });

            // Use the mock handler to create an HttpClient
            var mockHttpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://example.com")  // Set a base URI
            };


            // Mock CreateClientWithPolicy method to return the HttpClient
            _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(mockHttpClient);

            // Mock GetAdminAccessToken to return a valid token
            _mockCachingService.Setup(x => x.CacheItemExists(It.IsAny<string>())).Returns(false);
            _mockCachingService.Setup(x => x.SetCacheItem(It.IsAny<string>(), It.IsAny<string>(), null)).Verifiable();

            // Act
            var result = await _authenticationProvider.Login("adminUser", "adminPassword");

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(result.Value.Token, jwtToken);
            Assert.AreEqual(result.Value.UserId.ToString(), userId);


        }

        [Test]
        public async Task Login_ShouldReturnFailure_WhenSubIdWasNotSet()
        {
            var userId = "notvalid";
            var jwtToken = GenerateMockJwtToken(userId);
            // Mock HttpMessageHandler
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent($"{{\"access_token\": \"{jwtToken}\",\"refresh_token\": \"{{jwtToken}}\", \"expires_in\": 3600, \"refresh_expires_in\": 7200}}", Encoding.UTF8, "application/json")
                });

            // Use the mock handler to create an HttpClient
            var mockHttpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://example.com")  // Set a base URI
            };


            // Mock CreateClientWithPolicy method to return the HttpClient
            _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(mockHttpClient);

            // Mock GetAdminAccessToken to return a valid token
            _mockCachingService.Setup(x => x.CacheItemExists(It.IsAny<string>())).Returns(false);
            _mockCachingService.Setup(x => x.SetCacheItem(It.IsAny<string>(), It.IsAny<string>(), null)).Verifiable();

            // Act
            var result = await _authenticationProvider.Login("invalidUser", "wrongPassword");

            // Assert
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual(AuthenticationErrors.SubIdNotExist, result.Error);
        }

        [Test]
        public async Task Login_ShouldReturnFailure_WhenInvalidCredentials()
        {

            // Mock HttpMessageHandler
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Unauthorized))
                ;

            // Use the mock handler to create an HttpClient
            var mockHttpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://example.com")  // Set a base URI
            };


            // Mock CreateClientWithPolicy method to return the HttpClient
            _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(mockHttpClient);
            // Mock GetAdminAccessToken to return a valid token
            _mockCachingService.Setup(x => x.CacheItemExists(It.IsAny<string>())).Returns(false);
            _mockCachingService.Setup(x => x.SetCacheItem(It.IsAny<string>(), It.IsAny<string>(), null)).Verifiable();

            // Act
            var result = await _authenticationProvider.Login("invalidUser", "wrongPassword");

            // Assert
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual(AuthenticationErrors.InvalidUserNameOrPassword, result.Error);
        }

        [Test]
        public async Task GetIdByUserName_ShouldReturnUserId_WhenUserExists()
        {
            // Arrange
            var userName = "testuser";
            var expectedUserId = "123456";
            var token = "mocked_jwt_token";

            // Mock GetAdminAccessToken to return a successful result with a token
            _mockCachingService
                .Setup(x => x.CacheItemExists(It.IsAny<string>()))
                .Returns(false);

            _mockCachingService
                .Setup(x => x.GetCacheItem(It.IsAny<string>()))
                .Returns(token);

            // Create a mock HTTP client and set up response for GetAsync
            var responseContent = $"[{{\"id\": \"{expectedUserId}\"}}]";
            var _mockHandler = new Mock<HttpMessageHandler>();
            _mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
                });

            var mockHttpClient = new HttpClient(_mockHandler.Object) { BaseAddress = new Uri("https://example.com") };
            _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(mockHttpClient);

            // Act
            var result = await _authenticationProvider.GetIdByUserName(userName);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(expectedUserId, result.Value);
        }
    }


}