using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using Playlist_for_party.Data;
using Playlist_for_party.Interfaсes.Services.Managers.DataManagers;
using Playlist_for_party.Services.Managers.DataManagers;
using WebApp_Data.Models.Music;
using WebApp_Data.Models.UserData;
using Xunit;

namespace Playlist_for_party.Tests.Services.Managers.DataManagers
{
    public class PlaylistDataManagerTests
    {
        private readonly User _user;
        private readonly Playlist _playlist;
        private readonly Track _track;
        private readonly Mock<MusicContext> _mockContext;
        private readonly Mock<DbSet<Track>> _mockDbSet;
        private readonly Mock<IDataManager> _dataManagerMock;
        
        public PlaylistDataManagerTests()
        {
            _user = new User("test-userName", "test-Password");
            _playlist = new Playlist() { Name = "Test Playlist" , };
            _track = new Track { Id = "testTrackId", Name = "Test Track" };
            
            _mockContext = new Mock<MusicContext>();
            _mockDbSet = new Mock<DbSet<Track>>();
            _dataManagerMock = new Mock<IDataManager>();
        }

    }
}