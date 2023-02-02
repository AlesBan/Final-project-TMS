using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Playlist_for_party.Configuration;
using Playlist_for_party.Models;
using Playlist_for_party.Models.Authentication;

namespace Playlist_for_party.Controllers
{
    public class LoginController : Controller
    {
        private readonly JwtSettings _jwtSettings;

        public LoginController(IOptions<JwtSettings> options)
        {
            _jwtSettings = options.Value;
        }

        [HttpGet("registration")]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost("registration")]
        public IActionResult Registration(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            if (Startup.MusicRepository.Users.Any(u => u.UserName == user.UserName))
            {
                return BadRequest();
            }

            user.Roles.Add("user");
            Startup.MusicRepository.Users.Add(user);

            return Redirect("login");
        }

        [HttpGet("login")]
        public ActionResult<string> Login()
        {
            return View("Login");
        }

        [HttpPost("login")]
        public IActionResult Login(User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return (View(user));
                }

                if (!Startup.MusicRepository.Users.Any(u => u.UserName == user.UserName && u.Password == user.Password))
                {
                    return BadRequest();
                }

                var roles = new List<string>() { "user" };
                var token = Authentication.GenerateToken(_jwtSettings, user.UserName, roles);
                return RedirectToAction("Home", "Home", new { token });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}