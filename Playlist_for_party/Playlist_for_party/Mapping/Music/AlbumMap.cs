using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Playlist_for_party.Models;
using Playlist_for_party.Models.Music;

namespace Playlist_for_party.Mapping
{
    public class AlbumMap : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> builder)
        {
            builder.HasKey(b => b.AlbumId);
            
            builder.Property(b => b.AlbumId)
                .HasMaxLength(36);
            builder.Property(b => b.Title)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasMany(al => al.Songs)
                .WithOne(s => s.Album);
        }
    }
}