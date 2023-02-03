using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp_Data.Models.DbConnections;

namespace WebApp_Data.Mapping.Connections
{
    public class UserEditorPlaylistsMap : IEntityTypeConfiguration<UserEditorPlaylists>
    {
        public void Configure(EntityTypeBuilder<UserEditorPlaylists> builder)
        {
            builder.HasKey(ps => new { ps.PlaylistId, ps.UserEditorId });
            
        }
    }
}