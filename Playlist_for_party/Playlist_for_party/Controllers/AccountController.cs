using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApp_Data.Interfaces;
using WebApp_Data.Models;
using WebApp_Data.Models.Data;

namespace Playlist_for_party.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;

        public static IMusicRepository MusicRepository { get; set; } = new MusicRepository()
        {
            Users = new List<User>()
            {
                new User()
                {
                    UserId = Guid.Parse("596fcae8-7491-4940-b39c-8e86c2561dea"),
                    UserName = "ales",
                    Password = "ales"
                },
                new User()
                {
                    UserId = Guid.Parse("e24f63bc-a8eb-4fe3-a7d6-5844c1b30ab4"),
                    UserName = "pavel",
                    Password = "pavel"
                },
                new User()
                {
                    UserId = Guid.Parse("1afeeb58-2f69-422e-842d-0759a7b6825d"),
                    UserName = "dima",
                    Password = "dima"
                }
            }
        };

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

            if (MusicRepository.Users.Any(u => u.UserName == user.UserName))
            {
                return BadRequest();
            }

            user.Roles.Add("user");
            MusicRepository.Users.Add(user);

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

                if (!MusicRepository.Users.Any(u => u.UserName == userDto.UserName && u.Password == userDto.Password))
                {
                    return BadRequest();
                }

                var user = MusicRepository.GetUser(userDto);
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