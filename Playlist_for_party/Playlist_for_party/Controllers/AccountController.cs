using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Playlist_for_party.Exceptions.AppExceptions.DataExceptions;
using Playlist_for_party.Interfaсes.Services.Managers.DataManagers;
using Playlist_for_party.Interfaсes.Services.Managers.UserManagers;
using WebApp_Data.Models.DbConnections;
using WebApp_Data.Models.DTO;
using WebApp_Data.Models.UserData;

namespace Playlist_for_party.Controllers
{
    [Route("auth")]
    public class AccountController : Controller
    {
        private readonly IAuthManager _authManager;
        private readonly IConfiguration _configuration;
        private readonly IDataManager _dataManager;
        private const string FieldsMustBeEnteredMessage = "All fields must be entered ";
        private const string UserExistedMessage = "User with this username is existed";

        public AccountController(IAuthManager authManager, IConfiguration configuration,
            IDataManager dataManager)
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
            catch (SqlException)
            {
                throw new DataBaseConnectionException();
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
                ViewBag.ExceptionMessage = FieldsMustBeEnteredMessage;
                return View(singUpUserDto);
            }

            var usersDb = _dataManager.GetUsers();
            if (usersDb.Any(u => u.UserName == singUpUserDto.UserName))
            {
                ViewBag.ExceptionMessage = UserExistedMessage;
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

            var user = new User(singUpUserDto.UserName, singUpUserDto.Password);
            user.UserRoles.Add(new UserRole()
            {
                User = user,
                Role = new Role("User")
            });
            
            _dataManager.CreateUser(user);
            
            _authManager.SetToken(user, HttpContext, _configuration);

            return RedirectToAction("Home", "Home");
        }

        private IActionResult LoginValidation(UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return (View(userDto));
            }
            
            var usersDb = _dataManager.GetUsers();
            if (!usersDb.Any(u =>
                    u.UserName == userDto.UserName && u.Password == userDto.Password))
            {
                return BadRequest();
            }

            var user = _dataManager.GetUserByUserName(userDto.UserName);

            _authManager.SetToken(user, HttpContext, _configuration);

            return RedirectToAction("Home", "Home");
        }
    }
}