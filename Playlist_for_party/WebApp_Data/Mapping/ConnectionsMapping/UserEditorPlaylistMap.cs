using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp_Data.Models.DbConnections;
using WebApp_Data.Models.Music;

namespace WebApp_Data.Mapping.ConnectionsMapping
{
    public class UserEditorPlaylistMap : IEntityTypeConfiguration<UserEditorPlaylist>
    {
        public void Configure(EntityTypeBuilder<UserEditorPlaylist> builder)
        {
            builder.HasKey(up => new { up.PlaylistId, up.UserId });

            builder.HasOne(up => up.Playlist)
                .WithMany(p => p.UserEditorPlaylists)
                .HasForeignKey(pt => pt.PlaylistId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(ps => ps.User)
                .WithMany(p => p.UserEditorPlaylists)
                .HasForeignKey(pt => pt.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}