using Cdcn.Enterprise.Library.Application.Core.Abstraction.Authentication.Contracts;
using Cdcn.Enterprise.Library.Application.Core.Abstraction.Caching;
using Cdcn.Enterprise.Library.Infrastructure.Authentication;
using Cdcn.Enterprise.Library.Infrastructure.Authentication.Setting;
using Cdcn.Enterprise.Library.Infrastructure.Extension;
using MediatR;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
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

        [Test]
        public async Task Login_ShouldReturnSuccess_WhenValidCredentials()
        {

            // Arrange
            var expectedToken = new TokenResponse("access_token", "refresh_token", DateTime.UtcNow.AddMinutes(30), DateTime.UtcNow.AddMinutes(60), Guid.NewGuid());

            // Generate a mock JWT token (a simple one for testing purposes)
            var jwtHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes("{\"alg\":\"HS256\",\"typ\":\"JWT\"}"));
            var jwtPayload = Convert.ToBase64String(Encoding.UTF8.GetBytes("{\"sub\":\"" + expectedToken.Token + "\",\"exp\":" + (DateTime.UtcNow.AddMinutes(30).Ticks) + "}"));
            var jwtSignature = "mock_signature"; // For testing, the signature doesn't need to be valid

            var jwtToken = $"{jwtHeader}.{jwtPayload}.{jwtSignature}"; // Combine header, payload, and signature

            // Mock HttpMessageHandler
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent($"{{\"access_token\": \"{jwtToken}\", \"expires_in\": 3600, \"refresh_expires_in\": 7200}}", Encoding.UTF8, "application/json")
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
                _mockCachingService.Setup(x => x.SetCacheItem(It.IsAny<string>(), It.IsAny<string>(),null)).Verifiable();

                // Act
                var result = await _authenticationProvider.Login("adminUser", "adminPassword");

                // Assert
                Assert.IsTrue(result.IsSuccess);
                Assert.AreEqual(expectedToken.Token, result.Value.Token);
                _mockCachingService.Verify();
            
        }

        [Test]
        public async Task Login_ShouldReturnFailure_WhenInvalidCredentials()
        {
            // Arrange
            _mockHttpClientFactory.Setup(x => x.CreateClientWithPolicy())
                .Returns(new Mock<HttpClient>().Object);

            // Mock HttpClient failure
            _mockCachingService.Setup(x => x.CacheItemExists(It.IsAny<string>())).Returns(false);
            _mockCachingService.Setup(x => x.SetCacheItem(It.IsAny<string>(), It.IsAny<string>(), null)).Verifiable();

            // Act
            var result = await _authenticationProvider.Login("invalidUser", "wrongPassword");

            // Assert
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual(AuthenticationErrors.InvalidUserNameOrPassword, result.Error);
        }
    }


}