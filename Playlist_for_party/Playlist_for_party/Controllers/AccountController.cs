using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Playlist_for_party.Interfaсes.Services;
using WebApp_Data.Models;
using WebApp_Data.Models.Data;

namespace Playlist_for_party.Controllers
{
    [Route("auth")]
 
    public class AccountController : Controller
    {
        private readonly IAuthManager _authManager;
        private readonly IConfiguration _configuration;
        private readonly IMusicDataManagerService _dataManager;
        private const string FieldsMustBeEnteredMessage = "All fields must be entered ";
        
        public static readonly MusicRepository MusicRepository = new MusicRepository()
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
        
        public AccountController(IAuthManager authManager,IConfiguration configuration, IMusicDataManagerService dataManager)
        {
            _authManager = authManager;
            _configuration = configuration;
            _dataManager = dataManager;
        }

        [Route("singup")]
        public IActionResult SingUp()
        {
            return View();
        }

        [HttpPost("singup")]
        public IActionResult SingUp(SingUpUserDto singUpUserDto)
        {
            try
            {
                return SingUpValidation(singUpUserDto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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
                return LoginValidation(userDto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private IActionResult SingUpValidation(SingUpUserDto singUpUserDto)
        {
            if (string.IsNullOrEmpty(singUpUserDto.UserName) 
                || string.IsNullOrEmpty(singUpUserDto.Password) 
                || string.IsNullOrEmpty(singUpUserDto.ReEnterPassword))
            {
                ViewBag.ExceptionMessage = FieldsMustBeEnteredMessage ;
                return View(singUpUserDto);
            }
            
            try
            {
                _authManager.ValidateSingUpData(singUpUserDto);
            }
            catch (Exception e)
            {
                ViewBag.ExceptionMessage = e.Message;
                return View(singUpUserDto);
            }

            var user = new User()
            {
                UserId = Guid.NewGuid(),
                UserName = singUpUserDto.UserName,
                Password = singUpUserDto.Password
            };

            user.Roles.Add("user");

            _dataManager.AddUser(user);

            _authManager.SetToken(user, HttpContext, _configuration);

            return RedirectToRoute("home");
        }

        private IActionResult LoginValidation(UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return (View(userDto));
            }

            if (!MusicRepository.Users.Any(u =>
                    u.UserName == userDto.UserName && u.Password == userDto.Password))
            {
                return BadRequest();
            }

            var user = new User()
            {
                UserId = Guid.NewGuid(),
                UserName = userDto.UserName,
                Password = userDto.Password
            };
            
            _authManager.SetToken(user, HttpContext, _configuration);

            return RedirectToRoute("home");

        }
    }
}