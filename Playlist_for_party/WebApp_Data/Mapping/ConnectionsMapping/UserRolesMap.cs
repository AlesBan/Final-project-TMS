using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp_Data.Models.DbConnections;

namespace WebApp_Data.Mapping.ConnectionsMapping
{

    public class UserRolesMap : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(ur => new { ur.UserId, ur.RoleId });

            builder.HasOne(up => up.Role)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(pt => pt.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
 
            builder.HasOne(ps => ps.User)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(pt => pt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}