using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Playlist_for_party.Filters.ExceptionFilters;
using Playlist_for_party.Interfaсes.Services;
using WebApp_Authentication.Controllers;
using WebApp_Data.Models;
using WebApp_Data.Models.Music;

namespace Playlist_for_party.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        internal User GetCurrentUser()
        {
            var userId = Guid.Parse(HttpContext.Request.Cookies["UserId"]);
            return AccountController.MusicRepository.GetUser(userId);
        }
    }
}