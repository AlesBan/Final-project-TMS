using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Moq;
using Playlist_for_party.Interfaсes.Services;
using Playlist_for_party.Services;
using WebApp_Data.Models;
using WebApp_Data.Models.Music;
using Xunit;

namespace Playlist_for_party.Tests.Services
{
    public class UserManagerServiceTests
    {
        private readonly Mock<IMusicDataManagerService> _dataManagerMock;
        private readonly Mock<IMusicService> _musicServiceMock;
        private readonly UserManagerService _myService;
        private readonly User _user;
        private readonly Playlist _playlist;
        private Track _track;

        public UserManagerServiceTests()
        {
            _dataManagerMock = new Mock<IMusicDataManagerService>();
            _musicServiceMock = new Mock<IMusicService>();
            _myService = new UserManagerService(_dataManagerMock.Object, _musicServiceMock.Object);
            _user = new User()
            {
                UserId = Guid.NewGuid(),
                UserName = "test-name",
                Password = "test-password"
            };
            _playlist = new Playlist();
            _track = new Track();
        }
        
        [Fact]
        public void GetCurrentUser_ShouldReturnUser_WhenUserIdIsValid_OK()
        {
            // Arrange
            var context = new Mock<HttpContext>();
            var identity = new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()) });
            context.Setup(c => c.User).Returns(new ClaimsPrincipal(identity));

            var userId = identity.Claims.First().Value;
            var user = new User { UserId = Guid.Parse(userId) };
            _dataManagerMock.Setup(dm => dm.GetUser(user.UserId)).Returns(user);

            // Act
            var result = _myService.GetCurrentUser(context.Object);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user, result);
        }

        [Fact]
        public void SetRedactor_Adds_Redactor_To_Playlist_And_Playlist_To_User_OK()
        {
            // Arrange

            // Act
            _myService.SetRedactor(_user, _playlist);

            // Assert
            Assert.Contains(_user, _playlist.Redactors);
            Assert.Contains(_playlist, _user.PlaylistsAsRedactor);
        }

        [Fact]
        public void CreatePlaylist_Creates_Playlist_And_Sets_User_As_Owner_OK()
        {
            // Arrange
            _dataManagerMock.Setup(x => x.CreatePlaylist())
                .Returns(_playlist);
            
            // Act
            _myService.CreatePlaylist(_user);

            // Assert
            Assert.NotNull(_playlist);
            Assert.Equal(_user, _playlist.Owner);
            Assert.Contains(_playlist, _user.PlaylistsAsOwner);
        }

        [Fact]
        public void GetUserPlaylistAndTrack_Returns_User_Playlist_And_Track_OK()
        {
            // Arrange
            var context = new DefaultHttpContext();
            _musicServiceMock.Setup(ms => ms.GetTrack(It.IsAny<string>())).ReturnsAsync(_track);
            _dataManagerMock.Setup(dm => dm.GetPlaylists()).Returns(new List<Playlist> { _playlist });

            // Act
            Playlist resultPlaylist;
            Track resultTrack;
            _myService.GetPlaylistAndTrack(context, _track.TrackId, _playlist.PlaylistId.ToString(),
                out resultPlaylist, out resultTrack);

            // Assert
            Assert.Equal(_playlist, resultPlaylist);
            Assert.Equal(_track, resultTrack);
        }
    }
}