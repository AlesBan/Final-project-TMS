using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Playlist_for_party.Models;
using Playlist_for_party.Models.Music;

namespace Playlist_for_party.Mapping
{
    public class SongMap : IEntityTypeConfiguration<Song>
    {
        public void Configure(EntityTypeBuilder<Song> builder)
        {
            builder.HasKey(b => b.SongId);
            
            builder.Property(b => b.SongId)
                .HasMaxLength(36);
            builder.Property(b => b.Title)
                .HasMaxLength(50)
                .IsRequired();   
            builder.Property(b => b.ArtistId)
                .HasMaxLength(40)
                .IsRequired();
            builder.Property(b => b.AlbumId)
                .HasMaxLength(40)
                .IsRequired();
            builder.Property(b => b.Popularity)
                .HasDefaultValue(0);
            builder.Property(b => b.Duration)
                .HasDefaultValue(0);
            builder.Property(b => b.ImageRef)
                .HasMaxLength(50)
                .IsRequired();
            
        }
    }
}