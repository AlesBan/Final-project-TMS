using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Moq;
using Playlist_for_party.Exceptions.AppExceptions;
using Playlist_for_party.Interfaсes.Services.Managers.DataManagers;
using Playlist_for_party.Services.Managers.UserManagers;
using WebApp_Data.Models.UserData;
using Xunit;

namespace Playlist_for_party.Tests.Services
{
    public class UserManagerTests
    {
        private readonly Mock<IDataManager> _dataManagerMock;
        private readonly User _user;

        public UserManagerTests()
        {
            _dataManagerMock = new Mock<IDataManager>();
            _user = new User("test-name", "test-password");
        }

        [Fact]
        public void GetCurrentUser_ShouldReturnCurrentUser_OK()
        {
            // Arrange
            var claims = new[] { new Claim(ClaimTypes.Name, _user.UserName) };
            var context = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(claims))
            };

            _dataManagerMock.Setup(x => x.GetUserByUserName(_user.UserName)).Returns(_user);
            var userManager = new UserManager(_dataManagerMock.Object);

            // Act
            var resultUser = userManager.GetCurrentUser(context);

            // Assert
            Assert.Equal(_user, resultUser);
        }

        [Fact]
        public void GetCurrentUser_ShouldThrowInvalidClaimedUserIdException_WhenUserNameClaimIsMissing_FAIL()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var userManager = new UserManager(_dataManagerMock.Object);
            _dataManagerMock.Setup(x => x
                .GetUserByUserName(_user.UserName))
                .Returns(_user);

            // Act & Assert
            Assert.Throws<InvalidClaimedUserIdException>(() => userManager.GetCurrentUser(context));
            _dataManagerMock.Verify(x => x
                .GetUserByUserName(It.IsAny<string>()), Times.Never);
        }
    }
}