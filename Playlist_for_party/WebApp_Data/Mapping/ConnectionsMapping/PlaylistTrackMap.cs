using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp_Data.Models.DbConnections;

namespace WebApp_Data.Mapping.ConnectionsMapping
{
    public class PlaylistTrackMap : IEntityTypeConfiguration<PlaylistTrack>
    {
        public void Configure(EntityTypeBuilder<PlaylistTrack> builder)
        {
            builder.HasKey(ps => new { ps.PlaylistId, ps.TrackId });
            
            builder.HasOne(ps => ps.Playlist)
                .WithMany(p => p.PlaylistTracks)
                .HasForeignKey(pt => pt.PlaylistId)
                .OnDelete(DeleteBehavior.Cascade);
 
            builder.HasOne(ps => ps.Track)
                .WithMany(p => p.PlaylistTracks)
                .HasForeignKey(pt => pt.TrackId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}