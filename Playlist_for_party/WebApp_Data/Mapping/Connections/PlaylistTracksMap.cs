using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp_Data.Models.DbConnections;

namespace WebApp_Data.Mapping.Connections
{
    public class PlaylistTracksMap : IEntityTypeConfiguration<PlaylistTracks>
    {
        public void Configure(EntityTypeBuilder<PlaylistTracks> builder)
        {
            builder.HasKey(ps => new { ps.PlaylistId, ps.TrackId });
        }

    }
}