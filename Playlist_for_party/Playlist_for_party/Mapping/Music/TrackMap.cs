using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Playlist_for_party.Models;
using Playlist_for_party.Models.Music;

namespace Playlist_for_party.Mapping
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
            builder.Property(b => b.ImageRef)
                .HasMaxLength(80)
                .IsRequired();
            builder.Property(b => b.Album)
                .HasMaxLength(50)
                .IsRequired();            
            builder.Property(b => b.Artist)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}