using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Playlist_for_party.Services;
using WebApp_Data.Models;

namespace Playlist_for_party.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly MusicDataManagerService _dataManager = new MusicDataManagerService();

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("registration")]
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

            if (_dataManager.MusicRepository.Users.Any(u => u.UserName == user.UserName))
            {
                return BadRequest();
            }

            user.Roles.Add("user");
            _dataManager.MusicRepository.Users.Add(user);

            return Redirect("login");
        }

        [Route("login")]
        public ActionResult<string> Login()
        {
            return View("Login");
        }

        [HttpPost("login")]
        public IActionResult Login(UserDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return (View(userDto));
                }

                if (!_dataManager.MusicRepository.Users.Any(u => u.UserName == userDto.UserName && u.Password == userDto.Password))
                {
                    return BadRequest();
                }

                var user = _dataManager.GetUser(userDto);
                var roles = new List<string>() { "user" };
                var token = Authentication.Authentication.GenerateToken(_configuration, user, roles);
                
                HttpContext.Session.SetString("Authorization", token);

                return RedirectToAction("Home", "Home");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}