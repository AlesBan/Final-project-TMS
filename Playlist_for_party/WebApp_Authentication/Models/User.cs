using System;
using System.Collections.Generic;

namespace WebApp_Authentication.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string ImageRef { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}