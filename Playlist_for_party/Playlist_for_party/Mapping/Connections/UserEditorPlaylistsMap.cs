using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Playlist_for_party.Models.Connections;
using Playlist_for_party.Models.Music;

namespace Playlist_for_party.Mapping.Connections
{
    public class UserEditorPlaylistsMap : IEntityTypeConfiguration<UserEditorPlaylists>
    {
        public void Configure(EntityTypeBuilder<UserEditorPlaylists> builder)
        {
            builder.HasKey(ps => new { ps.PlaylistId, ps.UserEditorId });
            
            builder.HasOne(ps => ps.Playlist)
                .WithMany(p => p.UserEditorPlaylists)
                .HasForeignKey(ps => ps.PlaylistId);
            builder.HasOne(ps => ps.UserEditor)
                .WithMany(ue => ue.UserEditorPlaylists)
                .HasForeignKey(ps => ps.UserEditorId);
        }
    }
}