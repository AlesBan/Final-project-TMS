using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Playlist_for_party.Models.Connections;
using Playlist_for_party.Models.Music;

namespace Playlist_for_party.Mapping.Connections
{
    public class PlaylistSongsMap : IEntityTypeConfiguration<PlaylistSongs>
    {
        public void Configure(EntityTypeBuilder<PlaylistSongs> builder)
        {
            builder.HasKey(ps => new { ps.PlaylistId, ps.SongId });
            builder.HasOne(ps => ps.Playlist)
                .WithMany(p => p.PlaylistSongs)
                .HasForeignKey(ps => ps.PlaylistId);
            builder.HasOne(ps => ps.Song)
                .WithMany(s => s.PlaylistSongs)
                .HasForeignKey(ps => ps.SongId);
        }
    }
}