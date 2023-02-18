using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using Playlist_for_party.Exceptions.UserExceptions;
using Playlist_for_party.Services.Managers.UserManagers;
using WebApp_Data.Models.DTO;
using WebApp_Data.Models.UserData;
using Xunit;

namespace Playlist_for_party.Tests.Services
{
    public class AuthManagerTests
    {
        private readonly User _user;
        public AuthManagerTests()
        {
            _user = new User("test-name", "test-password");
        }

        [Fact]
        public void SetToken_Should_Set_Authorization_In_Session_OK()
        {
            //Arrange
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(x => x["JWTSettings:SecretKey"])
                .Returns("test-value-test-value-test-value-test-value-test-value");            
            configurationMock.Setup(x => x["JWTSettings:Issuer"])
                .Returns("test-value");            
            configurationMock.Setup(x => x["JWTSettings:Audience"])
                .Returns("test-value");
            
            var mockContextMock = new Mock<HttpContext>();
            var mockSessionMock = new Mock<ISession>();
            
            mockContextMock.SetupGet(x => x.Session)
                .Returns(mockSessionMock.Object);
            var service = new AuthManager();

            // Act
            service.SetToken(_user, mockContextMock.Object, 
                configurationMock.Object);

            // Assert
            mockSessionMock.Verify(x => x
                .Set("Authorization", It.IsAny<byte[]>()), Times.Once);
        }

        [Theory]
        [MemberData(nameof(UsersDtoSingUpFail))]
        public void ValidateSingUpData_ValidData_ExceptionThrow_FAIL(int num, SingUpUserDto singUpUserDto, Exception exception)
        {
            //Arrange
            var service = new AuthManager();
            
            //Act, Assert
            Assert.Throws(exception.GetType(), () => service.ValidateSingUpData(singUpUserDto));
        }
        
        [Theory]
        [MemberData(nameof(UsersDtoSetTokenOk))]
        public void ValidateSingUpData_ValidData_NoExceptionThrown_OK(SingUpUserDto singUpUserDto)
        {
            // Arrange
            var service = new AuthManager();

            // Act
            void SingUpAction()
            {
                service.ValidateSingUpData(singUpUserDto);
            }

            // Assert
            Assert.Null(Record.Exception(SingUpAction));
        }

        public static IEnumerable<object[]> UsersDtoSetTokenOk()
        {
            return new[]
            {
                new object[]
                {
                    new SingUpUserDto()
                    {
                        UserName = "ValidUserName",
                        Password = "ValidPassword123",
                        ReEnterPassword = "ValidPassword123"
                        
                    }
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
                    new SingUpUserDto()
                    {
                        UserName = "2",
                        Password = "qwerqwe",
                        ReEnterPassword = "qwerqwe"
                    },
                    new InvalidUserNameLengthException()
                },
                new object[]
                {
                    2,
                    new SingUpUserDto()
                    {
                        UserName = "qee3e3er",
                        Password = "2",
                        ReEnterPassword = "2"
                    },
                    new InvalidPasswordLengthException()
                },
                new object[]
                {
                    3,
                    new SingUpUserDto()
                    {
                        UserName = "qw!er",
                        Password = "qwerqwe",
                        ReEnterPassword = "qwerqwe"
                    },
                    new InvalidSingUpUserNameException()
                },
                new object[]
                {
                    4,
                    new SingUpUserDto()
                    {
                        UserName = "qw#er",
                        Password = "qwerqwe",
                        ReEnterPassword = "qwerqwe"
                    },
                    new InvalidSingUpUserNameException()
                },
                new object[]
                {
                    5,
                    new SingUpUserDto()
                    {
                        UserName = "q%er",
                        Password = "qwerqwe",
                        ReEnterPassword = "qwerqwe"
                    },
                    new InvalidSingUpUserNameException()
                },
                new object[]
                {
                    6,
                    new SingUpUserDto()
                    {
                        UserName = "q&swr",
                        Password = "qwerqwe",
                        ReEnterPassword = "qwerqwe"
                    },
                    new InvalidSingUpUserNameException()
                },
                new object[]
                {
                    7,
                    new SingUpUserDto()
                    {
                        UserName = "qw*er",
                        Password = "qwerqwe",
                        ReEnterPassword = "qwerqwe"
                    },
                    new InvalidSingUpUserNameException()
                },
                new object[]
                {
                    8,
                    new SingUpUserDto()
                    {
                        UserName = "qw+er",
                        Password = "qwerqwe",
                        ReEnterPassword = "qwerqwe"
                    },
                    new InvalidSingUpUserNameException()
                },
                new object[]
                {
                    9,
                    new SingUpUserDto()
                    {
                        UserName = "qwrerer",
                        Password = "qwerqwedwwe",
                        ReEnterPassword = "qweederqwe"
                    },
                    new PasswordConfirmationException()
                },
            };
        }
    }
}