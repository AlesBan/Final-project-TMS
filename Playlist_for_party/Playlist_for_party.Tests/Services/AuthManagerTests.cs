using System.Collections.Generic;
using WebApp_Data.Models;
using Xunit;

namespace Playlist_for_party.Tests.Services
{
    public class AuthManagerTests
    {
        [Fact]
        public void ValidateSingUpData_OK()
        {
        }

        [Theory]
        [MemberData(nameof(UserDtoSingUp_Fail))]
        public void ValidateSingUpData_Fail(UserDtoSingUp userDtoSingUp)
        {
            
        }

        public static IEnumerable<object[]> UserDtoSingUp_Fail()
        {
            return new[]
            {
                new object[]
                {
                    new UserDtoSingUp()
                    {
                        UserName = "qer",
                        Password = "2"
                    }
                },
                new object[]
                {
                    new UserDtoSingUp()
                    {
                        UserName = "2",
                        Password = "qw323r"
                    },
                },
                new object[]
                {
                    new UserDtoSingUp()
                    {
                        UserName = "qw!er",
                        Password = "qwerqwe"
                    },
                },
                new object[]
                {
                    new UserDtoSingUp()
                    {
                        UserName = "qw#er",
                        Password = "qwerqwe"
                    },
                },
                new object[]
                {
                    new UserDtoSingUp()
                    {
                        UserName = "q%er",
                        Password = "qwerqwe"
                    },
                },
                new object[]
                {
                    new UserDtoSingUp()
                    {
                        UserName = "q&swr",
                        Password = "qwerqwe"
                    },
                },
                new object[]
                {
                    new UserDtoSingUp()
                    {
                        UserName = "qw*er",
                        Password = "qwerqwe"
                    },
                },
                new object[]
                {
                    new UserDtoSingUp()
                    {
                        UserName = "qw+er",
                        Password = "qwerqwe"
                    },
                },
            };
        }
    }
}