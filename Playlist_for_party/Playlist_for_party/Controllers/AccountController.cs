using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Playlist_for_party.Exceptions.UserExceptions;
using Playlist_for_party.Interfaсes.Services;
using Playlist_for_party.Services;
using SpotifyAPI.Web.Models;
using WebApp_Data.Models;

namespace Playlist_for_party.Controllers
{
    [Route("auth")]

    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserManagerService _userManager;
        private readonly IAuthManager _authManager;
        private readonly MusicDataManagerService _dataManager = new MusicDataManagerService();
        private readonly string FieldsMustBeEnteredMessage = "All fields must be entered "; 
        public AccountController(
            IConfiguration configuration,
            IUserManagerService userManager,
            IAuthManager authManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _authManager = authManager;
        }

        [Route("singup")]
        public IActionResult SingUp()
        {
            return View();
        }

        [HttpPost("singup")]
        public IActionResult SingUp(UserDtoSingUp userDto)
        {
            return SingUpValidation(userDto);
        }

        [Route("login")]
        public ActionResult<string> Login()
        {
            return View("Login");
        }

        [HttpPost("login")]
        public IActionResult Login(UserDtoLogin userDto)
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

        private IActionResult SingUpValidation(UserDtoSingUp userDto)
        {
            if (!ModelState.IsValid)
            {
                return View(userDto);
            }

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

            if (_dataManager.MusicRepository.Users.Any(u => u.UserName == userDto.UserName))
            {
                return BadRequest();
            }

            var user = new User()
            {
                UserName = userDto.UserName,
                Password = userDto.Password
            };

            user.Roles.Add("user");

            _dataManager.MusicRepository.Users.Add(user);

            return Redirect("login");
        }

        private IActionResult LoginValidation(UserDtoLogin userDto)
        {
            if (!ModelState.IsValid)
            {
                return (View(userDto));
            }

            if (!_dataManager.MusicRepository.Users.Any(u =>
                    u.UserName == userDto.UserName && u.Password == userDto.Password))
            {
                return BadRequest();
            }

            _authManager.SetToken(userDto, HttpContext, _userManager, _configuration);

            return RedirectToAction("Home", "Home");
        }
    }
}