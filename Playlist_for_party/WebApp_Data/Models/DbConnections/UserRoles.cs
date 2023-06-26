using System;
using WebApp_Data.Models.Music;
using WebApp_Data.Models.UserData;

namespace WebApp_Data.Models.DbConnections
{
    public class UserRole
    {
        public Guid UserId { get; set; }
        public User User{ get; set; }
        public Guid RoleId  { get; set; }
        public Role Role  { get; set; }
    }
}