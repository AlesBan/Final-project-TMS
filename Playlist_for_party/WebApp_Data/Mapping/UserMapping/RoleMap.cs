using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp_Data.Models.UserData;

namespace WebApp_Data.Mapping.UserMapping
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id)
                .HasColumnName("RoleId")
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.Name)
                .HasMaxLength(10);
        }
    }
}