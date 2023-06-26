using System.ComponentModel.DataAnnotations;

namespace WebApp_Data.Models.DTO
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