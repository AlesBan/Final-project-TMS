using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Playlist_for_party.Models.DbConnections;
using Playlist_for_party.Models.Music;

namespace Playlist_for_party.Mapping.Connections
{
    public class PlaylistTracksMap : IEntityTypeConfiguration<PlaylistTracks>
    {
        public void Configure(EntityTypeBuilder<PlaylistTracks> builder)
        {
            builder.HasKey(ps => new { ps.PlaylistId, ps.TrackId });
            builder.HasOne(ps => ps.Playlist)
                .WithMany(p => p.PlaylistTracks)
                .HasForeignKey(ps => ps.PlaylistId);
            builder.HasOne(ps => ps.Track)
                .WithMany(s => s.PlaylistTracks)
                .HasForeignKey(ps => ps.TrackId);
        }
    }
}