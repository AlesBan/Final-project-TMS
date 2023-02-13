using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using Playlist_for_party.Exceptions.UserExceptions;
using Playlist_for_party.Interfaсes.Services;
using Playlist_for_party.Services;
using WebApp_Data.Models;
using Xunit;

namespace Playlist_for_party.Tests.Services
{
    public class AuthManagerServiceTests
    {
        [Fact]
        public void SetToken_Should_Set_Authorization_In_Session_OK()
        {
            //Arrange
            var userDto = new UserDtoLogin { UserName = "test@example.com", Password = "testPassword" };
            const string token = "testToken";
            var decodedToken = Encoding.UTF8.GetBytes(token);
            var configuration = new ConfigurationBuilder().Build();
            var mockContext = new Mock<HttpContext>();
            var mockSession = new Mock<ISession>();
            var mockUserManager = new Mock<IUserManagerService>();
            mockUserManager.Setup(x => x.CreateToken(userDto, configuration))
                .Returns(token);
            mockContext.SetupGet(x => x.Session).Returns(mockSession.Object);
            var service = new AuthManager(mockUserManager.Object);

            // Act
            service.SetToken(userDto, mockContext.Object, configuration);

            // Assert
            mockSession.Verify(x => x.Set("Authorization", decodedToken), Times.Once);
        }

        [Theory]
        [MemberData(nameof(UsersDtoSingUpFail))]
        public void ValidateSingUpData_Fail(int num, UserDtoSingUp userDtoSingUp, Exception exception)
        {
            //Arrange
            var manager = new Mock<IUserManagerService>();
            var service = new AuthManager(manager.Object);
            //Act, Assert
            Assert.Throws(exception.GetType(), () => service.ValidateSingUpData(userDtoSingUp));
        }

        public static IEnumerable<object[]> UsersDtoSetTokenOk()
        {
            return new[]
            {
                new object[]
                {
                    new UserDtoLogin()
                    {
                        UserName = "23423",
                        Password = "qw323r"
                    },
                    new DefaultHttpContext()
                },
            };
        }

        public static IEnumerable<object[]> UsersDtoSingUpFail()
        {
            return new[]
            {
                new object[]
                {
                    1,
                    new UserDtoSingUp()
                    {
                        UserName = "2",
                        Password = "qw323r"
                    },
                    new InvalidUserNameLengthException()
                },
                new object[]
                {
                    2,
                    new UserDtoSingUp()
                    {
                        UserName = "qee3e3er",
                        Password = "2"
                    },
                    new InvalidPasswordLengthException()
                },
                new object[]
                {
                    3,
                    new UserDtoSingUp()
                    {
                        UserName = "qw!er",
                        Password = "qwerqwe"
                    },
                    new InvalidSingUpUserNameException()
                },
                new object[]
                {
                    4,
                    new UserDtoSingUp()
                    {
                        UserName = "qw#er",
                        Password = "qwerqwe"
                    },
                    new InvalidSingUpUserNameException()
                },
                new object[]
                {
                    5,
                    new UserDtoSingUp()
                    {
                        UserName = "q%er",
                        Password = "qwerqwe"
                    },
                    new InvalidSingUpUserNameException()
                },
                new object[]
                {
                    6,
                    new UserDtoSingUp()
                    {
                        UserName = "q&swr",
                        Password = "qwerqwe"
                    },
                    new InvalidSingUpUserNameException()
                },
                new object[]
                {
                    7,
                    new UserDtoSingUp()
                    {
                        UserName = "qw*er",
                        Password = "qwerqwe"
                    },
                    new InvalidSingUpUserNameException()
                },
                new object[]
                {
                    8,
                    new UserDtoSingUp()
                    {
                        UserName = "qw+er",
                        Password = "qwerqwe"
                    },
                    new InvalidSingUpUserNameException()
                },
            };
        }
    }
}