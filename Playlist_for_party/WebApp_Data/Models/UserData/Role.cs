using System;
using System.Collections.Generic;
using WebApp_Data.Models.DbConnections;

namespace WebApp_Data.Models.UserData
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public Role(string name)
        {
            Name = name;
            Id = Guid.NewGuid();
        }
    }
}