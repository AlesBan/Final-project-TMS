using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Playlist_for_party.Models;
using Playlist_for_party.Models.Music;

namespace Playlist_for_party.Mapping
{
    public class PlaylistMap : IEntityTypeConfiguration<Playlist>
    {
        public void Configure(EntityTypeBuilder<Playlist> builder)
        {
            builder.HasKey(b => b.PlaylistId);
            builder.Property(b => b.PlaylistId)
                .HasMaxLength(40);
            builder.Property(b => b.Title)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(b => b.UrlRef)
                .HasMaxLength(50)
                .IsRequired();
            

        }
    }
}