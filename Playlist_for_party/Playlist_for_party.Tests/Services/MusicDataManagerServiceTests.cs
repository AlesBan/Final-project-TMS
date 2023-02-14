using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Playlist_for_party.Services;
using WebApp_Data.Interfaces;
using WebApp_Data.Models;
using WebApp_Data.Models.Music;
using Xunit;

namespace Playlist_for_party.Tests.Services
{
    public class MusicDataManagerServiceTests
    {
        [Fact]
        public void GetPlaylist_ShouldReturnCorrectPlaylist_OK()
        {
            // Arrange
            var playlistId = Guid.NewGuid();
            var expectedPlaylist = new Playlist { PlaylistId = playlistId };
            var musicRepository = new Mock<IMusicRepository>();
            musicRepository.Setup(x => x.Playlists).Returns(new List<Playlist> { expectedPlaylist });
            var musicService = new MusicDataManagerService();

            // Act
            var result = musicService.GetPlaylist(playlistId);

            // Assert
            Assert.Equal(expectedPlaylist, result);
        }
        
        [Fact]
        public void GetPlaylist_InvalidPlaylistId_ReturnsNull_FAIL()
        {
            // Arrange
            var musicRepository = new Mock<IMusicRepository>();
            musicRepository.Setup(repo => repo.Playlists)
                .Returns(new List<Playlist>());
            var sut = new MusicDataManagerService();

            // Act
            var actualPlaylist = sut.GetPlaylist(Guid.NewGuid());

            // Assert
            Assert.Null(actualPlaylist);
        }

        [Fact]
        public void GetPlaylists_ShouldReturnAllPlaylists_OK()
        {
            // Arrange
            var expectedPlaylists = new List<Playlist> { new Playlist(), new Playlist() };
            var musicRepository = new Mock<IMusicRepository>();
            musicRepository.Setup(x => x.Playlists).Returns(expectedPlaylists);
            var musicService = new MusicDataManagerService();

            // Act
            var result = musicService.GetPlaylists();

            // Assert
            Assert.Equal(expectedPlaylists, result);
        }

        [Fact]
        public void AddTrack_ShouldAddTrackToPlaylist_OK()
        {
            // Arrange
            var user = new User();
            var track = new Track();
            var playlist = new Playlist();
            var musicRepository = new Mock<IMusicRepository>();
            musicRepository.Setup(x => x.Playlists).Returns(new List<Playlist> { playlist });
            var musicService = new MusicDataManagerService();

            // Act
            musicService.AddTrack(user, playlist, track);

            // Assert
            Assert.Contains(track, playlist.UserTracks.Values.SelectMany(x => x));
        }

        [Fact]
        public void CreatePlaylist_ShouldCreateNewPlaylist_OK()
        {
            // Arrange
            var musicRepository = new Mock<IMusicRepository>();
            musicRepository.Setup(x => x.Playlists).Returns(new List<Playlist>());
            var musicService = new MusicDataManagerService();

            // Act
            var result = musicService.CreatePlaylist();

            // Assert
            Assert.Contains(result, musicRepository.Object.Playlists);
        }
    }
}