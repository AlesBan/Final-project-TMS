using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApp_Data.Models.Music;

namespace WebApp_Data.Models
{
    public class SingUpUserDto
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        public string ReEnterPassword { get; set; }
    }
}