using Cdcn.Enterprise.Library.Application.Core.Abstraction.Authentication;
using Cdcn.Enterprise.Library.Application.Core.Abstraction.Authentication.Events;
using Cdcn.Enterprise.Library.Application.Core.Abstraction.Caching;
using Cdcn.Enterprise.Library.Domain.Primitives.Result;
using Cdcn.Enterprise.Library.Infrastructure.Authentication;
using Cdcn.Enterprise.Library.Infrastructure.Authentication.Setting;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cdcn.Enterprise.Library.Tests.Infrastructure.Authentication
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        private Mock<IAuthenticationProvider> _mockAuthenticationProvider;
        private Mock<ICacheService> _mockCachingService;
        private Mock<IMediator> _mockMediator;
        private Mock<IOptions<AuthenticationSetting>> _mockAuthenticationSetting;
        private AuthenticationService _authenticationService;
        private Mock<ILogger<AuthenticationService>> _mockLogger;

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
            _mockAuthenticationProvider = new Mock<IAuthenticationProvider>();

            // Mock ICachingService
            _mockCachingService = new Mock<ICacheService>();

            // Mock IMediator
            _mockMediator = new Mock<IMediator>();

            // MOck ILOgger
            _mockLogger = new Mock<ILogger<AuthenticationService>>();

            // Create instance of AuthenticationProvider
            _authenticationService = new AuthenticationService(
                _mockAuthenticationProvider.Object,
                _mockCachingService.Object,
                _mockMediator.Object,
                _mockLogger.Object
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

            var mockToken = $"{{\"access_token\": \"{jwtToken}\",\"refresh_token\": \"{jwtToken}\", \"expires_in\": 3600, \"refresh_expires_in\": 7200}}";
            var expectedResult = Result.Success<string>(mockToken);

            // Mock CreateClientWithPolicy method to return the HttpClient
            _mockAuthenticationProvider.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(expectedResult);

            // Mock GetAdminAccessToken to return a valid token
            _mockCachingService.Setup(x => x.CacheItemExists(It.IsAny<string>())).Returns(false);
            _mockCachingService.Setup(x => x.SetCacheItem(It.IsAny<string>(), It.IsAny<string>(), null)).Verifiable();

            // Act
            var result = await _authenticationService.Login("adminUser", "adminPassword");

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(result.Value.Token, jwtToken);
            Assert.AreEqual(result.Value.UserId.ToString(), userId);
            _mockMediator.Verify(m => m.Publish(It.Is<UserLogedInProviderEvent>(e => e.UserName == "adminUser" && e.IsAuthenticated == true), default), Times.Once);


        }

        [Test]
        public async Task Login_ShouldReturnFailure_WhenSubIdWasNotSet()
        {
            var userId = "invalidUser";
            var jwtToken = GenerateMockJwtToken(userId);
            // Mock HttpMessageHandler
            var mockToken = $"{{\"access_token\": \"{jwtToken}\",\"refresh_token\": \"{jwtToken}\", \"expires_in\": 3600, \"refresh_expires_in\": 7200}}";
            var expectedResult = Result.Failure<string>(AuthenticationErrors.InvalidUserNameOrPassword);

            // Mock CreateClientWithPolicy method to return the HttpClient
            _mockAuthenticationProvider.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(expectedResult);

            // Mock GetAdminAccessToken to return a valid token
            _mockCachingService.Setup(x => x.CacheItemExists(It.IsAny<string>())).Returns(false);
            _mockCachingService.Setup(x => x.SetCacheItem(It.IsAny<string>(), It.IsAny<string>(), null)).Verifiable();

            // Act
            var result = await _authenticationService.Login("invalidUser", "wrongPassword");

            // Assert
            Assert.IsTrue(result.IsFailure);

            _mockMediator.Verify(m => m.Publish(It.Is<UserLogedInProviderEvent>(e => e.UserName == "invalidUser" && e.IsAuthenticated == false), default), Times.Once);
        }

        [Test]
        public async Task Login_ShouldReturnFailure_WhenInvalidCredentials()
        {

            // Mock HttpMessageHandler
            var mockToken = $"";
            var expectedResult = Result.Failure<string>(AuthenticationErrors.InvalidUserNameOrPassword);

            // Mock CreateClientWithPolicy method to return the HttpClient
            _mockAuthenticationProvider.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(expectedResult);
            // Mock GetAdminAccessToken to return a valid token
            _mockCachingService.Setup(x => x.CacheItemExists(It.IsAny<string>())).Returns(false);
            _mockCachingService.Setup(x => x.SetCacheItem(It.IsAny<string>(), It.IsAny<string>(), null)).Verifiable();

            // Act
            var result = await _authenticationService.Login("invalidUser", "wrongPassword");

            // Assert
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual(AuthenticationErrors.InvalidUserNameOrPassword, result.Error);
            _mockMediator.Verify(m => m.Publish(It.Is<UserLogedInProviderEvent>(e => e.UserName == "invalidUser" && e.IsAuthenticated == false), default), Times.Once);
        }

        [Test]
        public async Task LoginWithRefreshToken_ShouldReturnSuccess_WhenValidCredentials()
        {
            var userId = Guid.NewGuid().ToString();
            var jwtToken = GenerateMockJwtToken(userId);

            var mockToken = $"{{\"access_token\": \"{jwtToken}\",\"refresh_token\": \"{jwtToken}\", \"expires_in\": 3600, \"refresh_expires_in\": 7200}}";
            var expectedResult = Result.Success<string>(mockToken);

            // Mock CreateClientWithPolicy method to return the HttpClient
            _mockAuthenticationProvider.Setup(x => x.Login(It.IsAny<string>())).ReturnsAsync(expectedResult);

            // Mock GetAdminAccessToken to return a valid token
            _mockCachingService.Setup(x => x.CacheItemExists(It.IsAny<string>())).Returns(false);
            _mockCachingService.Setup(x => x.SetCacheItem(It.IsAny<string>(), It.IsAny<string>(), null)).Verifiable();

            // Act
            var result = await _authenticationService.LoginFromRefreshToken(jwtToken);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(result.Value.Token, jwtToken);
            Assert.AreEqual(result.Value.UserId.ToString(), userId);
            _mockMediator.Verify(m => m.Publish(It.Is<UserLogedInProviderEvent>(e => e.UserName == "" && e.IsAuthenticated == true), default), Times.Once);


        }

        [Test]
        public async Task CreateUser_ShouldReturnFailure_WhenUserNameAlreadyExists()
        {
            // Arrange
            var username = "testuser";
            var email = "testuser@example.com";
            var firstName = "Test";
            var lastName = "User";
            var password = "password";
            _mockAuthenticationProvider.Setup(x => x.IsUserNameExist(username))
                .ReturnsAsync(Result.Success(true));

            // Act
            var result = await _authenticationService.CreateUser(username, email, firstName, lastName, password);

            // Assert
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual(AuthenticationErrors.UserNameAlreadyExist, result.Error);
        }

        [Test]
        public async Task CreateUser_ShouldReturnFailure_WhenAdminAccessTokenIsInvalid()
        {
            // Arrange
            var username = "testuser";
            var email = "testuser@example.com";
            var firstName = "Test";
            var lastName = "User";
            var password = "password";

            var mockGetAdminAccessTokenResponse = Result.Failure<string>(AuthenticationErrors.InvalidUserNameOrPassword);

            _mockAuthenticationProvider.Setup(x => x.IsUserNameExist(username))
                .ReturnsAsync(Result.Success(false));
            _mockAuthenticationProvider.Setup(x => x.GetAdminAccessToken())
                .ReturnsAsync(mockGetAdminAccessTokenResponse);

            // Act
            var result = await _authenticationService.CreateUser(username, email, firstName, lastName, password);

            // Assert
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual(AuthenticationErrors.InvalidAdminToken, result.Error);
        }

        [Test]
        public async Task CreateUser_ShouldReturnSuccess_WhenUserIsCreatedSuccessfully()
        {
            // Arrange
            var username = "testuser";
            var email = "testuser@example.com";
            var firstName = "Test";
            var lastName = "User";
            var password = "password";
            var userId = Guid.NewGuid().ToString();
            var jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI1NTBlODQwMC1lMjliLTQxZDQtYTcxNi00NDY2NTU0NDAwMDAiLCJuYW1lIjoiSm9obiBEb2UiLCJpYXQiOjE1MTYyMzkwMjJ9.SidrxJjWYmR8cviBMT6wLny5TlabpHJ92FxQwMfJxeo";
            var mockToken = $"{{\"access_token\": \"{jwtToken}\",\"refresh_token\": \"{jwtToken}\", \"expires_in\": 3600, \"refresh_expires_in\": 7200}}";


            _mockAuthenticationProvider.Setup(x => x.IsUserNameExist(username))
                .ReturnsAsync(Result.Success(false));
            _mockAuthenticationProvider.Setup(x => x.GetAdminAccessToken())
                .ReturnsAsync(Result.Success(mockToken));
            _mockAuthenticationProvider.Setup(x => x.CreateUser(username, email, firstName, lastName, password))
                .ReturnsAsync(Result.Success(mockToken));
            _mockAuthenticationProvider.Setup(x => x.Login(username, password))
                .ReturnsAsync(Result.Success(mockToken));

            // Act
            var result = await _authenticationService.CreateUser(username, email, firstName, lastName, password);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

    }
}