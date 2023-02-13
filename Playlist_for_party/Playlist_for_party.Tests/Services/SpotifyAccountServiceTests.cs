using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using Playlist_for_party.Interfaсes.Services;
using Playlist_for_party.Services;
using WebApp_Data.Models.SpotifyModels.DTO;
using WebApp_Data.Models.SpotifyModels.Main;
using Xunit;

namespace Playlist_for_party.Tests.Services
{
    public class SpotifyAccountServiceTests
    {
        // [Fact]
        // public async Task GetItems_Ok_ReturnsItemsDto()
        // {
        //     // Arrange
        //     var query = "query";
        //     var accessToken = "accessToken";
        //     var responseObject = new Search();
        //     var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        //     {
        //         Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(responseObject))
        //     };
        //
        //     var mockSpotifyAccountService = new Mock<ISpotifyAccountService>();
        //     mockSpotifyAccountService
        //         .Setup(x => x.GetAccessToken())
        //         .ReturnsAsync(accessToken);
        //
        //     var mockHttpClient = new Mock<HttpClient>();
        //     mockHttpClient
        //         .Setup(x => x.GetAsync(It.IsAny<string>()))
        //         .ReturnsAsync(response);
        //
        //     var service = new SpotifyService(mockHttpClient.Object, mockSpotifyAccountService.Object);
        //
        //     // Act
        //     var result = await service.GetItems(query);
        //
        //     // Assert
        //     Assert.NotNull(result);
        // }
    }
}