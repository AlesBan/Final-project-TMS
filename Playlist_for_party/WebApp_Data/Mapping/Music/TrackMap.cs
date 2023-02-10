using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp_Data.Models.Music;

namespace WebApp_Data.Mapping.Music
{
    public class TrackMap : IEntityTypeConfiguration<Track>
    {
        public void Configure(EntityTypeBuilder<Track> builder)
        {
            builder.HasKey(b => b.TrackId);
            
            builder.Property(b => b.TrackId)
                .HasMaxLength(36);
            builder.Property(b => b.Name)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(b => b.Popularity)
                .HasDefaultValue(0);
            builder.Property(b => b.Duration)
                .HasDefaultValue(0);
            builder.Property(b => b.ImageUrl)
                .HasMaxLength(80)
                .IsRequired();
            builder.Property(b => b.Album)
                .HasMaxLength(50)
                .IsRequired();            
            builder.Property(b => b.ArtistName)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}