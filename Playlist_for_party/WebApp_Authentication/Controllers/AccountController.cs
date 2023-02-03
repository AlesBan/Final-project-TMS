using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApp_Authentication.Configuration;
using WebApp_Data.Interfaces;
using WebApp_Data.Models;
using WebApp_Authentication.Models.Authentication;
using WebApp_Data.Models.Data;

namespace WebApp_Authentication.Controllers
{
    public class AccountController : Controller
    {
        private readonly JwtSettings _jwtSettings;

        public static IMusicRepository MusicRepository { get; set; } = new MusicRepository();

        public AccountController(IOptions<JwtSettings> options)
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

            if (MusicRepository.Users.Any(u => u.UserName == user.UserName))
            {
                return BadRequest();
            }

            user.Roles.Add("user");
            MusicRepository.Users.Add(user);

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

                if (!MusicRepository.Users.Any(u => u.UserName == user.UserName && u.Password == user.Password))
                {
                    return BadRequest();
                }

                var roles = new List<string>() { "user" };
                var token = Authentication.GenerateToken(_jwtSettings, user.UserName, roles);
                HttpContext.Response.Cookies.Append("JWToken", token);
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