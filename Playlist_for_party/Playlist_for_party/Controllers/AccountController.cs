using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Playlist_for_party.Exceptions.UserExceptions;
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
        public IActionResult SingUp(string userName, string password)
        {
            // var userDto = Unosquare.Swan.Formatters.Json.Deserialize<UserDto>(receivedUserDto);
            var userDto = new UserDto()
            {
                UserName = userName,
                Password = password
            };
            
            try
            {
                return SingUpValidation(userDto);
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

        private IActionResult SingUpValidation(UserDto userDto)
        {
            if (string.IsNullOrEmpty(userDto.UserName) || string.IsNullOrEmpty(userDto.Password))
            {
                ViewBag.ExceptionMessage = FieldsMustBeEnteredMessage ;
                return View(userDto);
            }
            
            try
            {
                _authManager.ValidateSingUpData(userDto);
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case InvalidUserNameLengthException _:
                        ModelState.AddModelError("UserName", e.Message);
                        break;
                    
                    case InvalidSingUpUserNameException _:
                        ModelState.AddModelError("UserName", e.Message);
                        break;
                    
                    case InvalidPasswordLengthException _:
                        ModelState.AddModelError("UserName", e.Message);
                        break;
                    
                    default: 
                        ModelState.AddModelError("UserName", e.Message);
                        break;
                }

                ViewBag.ExceptionMessage = e.Message;
                return View(userDto);
            }

            if (MusicRepository.Users.Any(u => u.UserName == userDto.UserName))
            {
                return BadRequest();
            }

            var user = new User()
            {
                UserId = Guid.NewGuid(),
                UserName = userDto.UserName,
                Password = userDto.Password
            };

            user.Roles.Add("user");

            _dataManager.AddUser(user);

            _authManager.SetToken(userDto, HttpContext, _configuration);

            return RedirectToAction("Home", "Home");
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

            _authManager.SetToken(userDto, HttpContext, _configuration);

            return RedirectToAction("Home", "Home");
        }
    }
}