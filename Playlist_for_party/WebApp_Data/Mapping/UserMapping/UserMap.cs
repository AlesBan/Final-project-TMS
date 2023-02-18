using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp_Data.Models.UserData;

namespace WebApp_Data.Mapping.UserMapping
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                .HasColumnName("UserId")
                .HasDefaultValueSql("NEWID()");
            
            builder.Property(u => u.UserName)
                .HasMaxLength(50)
                .IsRequired();   
            builder.Property(u => u.Password)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(u => u.Email)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(u => u.ImageRef)
                .HasMaxLength(50);
            
            builder.HasMany(p => p.UserOwnerPlaylists)
                .WithOne(u => u.Owner)
                .HasForeignKey(p => p.OwnerId);
        }
    }
}