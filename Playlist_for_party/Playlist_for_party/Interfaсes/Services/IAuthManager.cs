using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApp_Data.Models;

namespace Playlist_for_party.Interfaсes.Services
{
    public interface IAuthManager
    {
        void SetToken(User user, HttpContext context, IConfiguration configuration);
        void ValidateSingUpData(SingUpUserDto singUpUserDto);
    }
}