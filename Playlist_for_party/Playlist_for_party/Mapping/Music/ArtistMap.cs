using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Playlist_for_party.Models;
using Playlist_for_party.Models.Music;

namespace Playlist_for_party.Mapping
{
    public class ArtistMap : IEntityTypeConfiguration<Artist>
    {
        public void Configure(EntityTypeBuilder<Artist> builder)
        {
            builder.HasKey(a => a.ArtistId);

            builder.Property(a => a.ArtistId)
                .HasMaxLength(36);
            builder.Property(a => a.ArtistName)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(a => a.ImageRef)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasMany(a => a.Songs)
                .WithOne(a => a.Artist);
            builder.HasMany(a => a.Albums)
                .WithOne(al => al.Artist);
        }
    }
}