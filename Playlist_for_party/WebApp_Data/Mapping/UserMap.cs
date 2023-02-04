using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp_Data.Models;

namespace WebApp_Data.Mapping
{
    public partial class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(b => b.UserId);
            
            builder.Property(b => b.UserId)
                .HasMaxLength(40);
            builder.Property(b => b.UserName)
                .HasMaxLength(50)
                .IsRequired();   
            builder.Property(b => b.Password)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(b => b.Email)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(b => b.ImageRef)
                .HasMaxLength(50);
            
        }
    }
}