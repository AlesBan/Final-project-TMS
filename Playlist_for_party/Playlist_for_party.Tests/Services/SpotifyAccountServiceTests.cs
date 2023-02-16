using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using Playlist_for_party.Exceptions.AppExceptions;
using Playlist_for_party.Models.SpotifyApiConnection;
using Playlist_for_party.Services;
using Xunit;
    using Microsoft.Extensions.Configuration;
using Moq.Protected;
using Playlist_for_party.Interfaсes.Services;

namespace Playlist_for_party.Tests.Services
{
    public class SpotifyAccountServiceTests
    {
        [Fact]
        public async Task GetAccessToken_ShouldReturnAccessToken()
        {
            // Arrange
            const string clientId = "test-clientSecret";
            const string clientSecret = "test-clientSecret";
            const string expectedAccessToken = "test-expectedAccessToken";
        
            var configurationMock = new Mock<IConfiguration>();
            configurationMock
                .SetupGet(c => c["Spotify:ClientId"])
                .Returns(clientId);
            configurationMock
                .SetupGet(c => c["Spotify:ClientSecret"])
                .Returns(clientSecret);
        
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedAccessToken)
            };
            
            var httpClientMock = new Mock<HttpClient>();
            
            httpClientMock.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(httpResponse);
            
            var spotifyAccountService =
                new SpotifyAccountService(httpClientMock.Object, configurationMock.Object);
        
            // Act
            var resultAccessToken = await spotifyAccountService.GetAccessToken();
        
            // Assert
            Assert.NotNull(resultAccessToken);
            Assert.NotEmpty(resultAccessToken);
        }
    }
}