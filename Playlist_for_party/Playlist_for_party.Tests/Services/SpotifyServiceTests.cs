using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using Playlist_for_party.Interfaсes.Services;
using Playlist_for_party.Services;
using WebApp_Data.Models.SpotifyModels.DTO;
using Xunit;

namespace Playlist_for_party.Tests.Services
{
    public class SpotifyServiceTests
    {
        // [Fact]
        // public async Task GetItems_ReturnsItemsDto_WhenCalledWithValidQuery()
        // {
        //     // Arrange
        //     const string query = "artist:Ariana Grande";
        //     var expectedResult = new ItemsDto();
        //
        //     var mockHttpClient = new Mock<HttpClient>();
        //     var mockSpotifyAccountService = new Mock<ISpotifyAccountService>();
        //
        //     var searchResponse = new HttpResponseMessage
        //     {
        //         StatusCode = HttpStatusCode.OK,
        //         Content = new StringContent(JsonConvert.SerializeObject(expectedResult))
        //     };
        //
        //     mockHttpClient.Setup(x => x.GetAsync(It.IsAny<string>()))
        //         .ReturnsAsync(searchResponse);
        //
        //     mockSpotifyAccountService.Setup(x => x.GetAccessToken())
        //         .ReturnsAsync("test_access_token");
        //
        //     var sut = new SpotifyService(mockHttpClient.Object, mockSpotifyAccountService.Object);
        //
        //     // Act
        //     var result = await sut.GetItems(query);
        //
        //     // Assert
        //     Assert.Equal(expectedResult, result);
        // }
    }
}