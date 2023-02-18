using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp_Data.Models.Music;

namespace WebApp_Data.Mapping.MusicMapping
{
    public class PlaylistMap : IEntityTypeConfiguration<Playlist>
    {
        public void Configure(EntityTypeBuilder<Playlist> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .HasColumnName("PlaylistId")
                .HasDefaultValueSql("NEWID()");

            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(p => p.Href)
                .HasMaxLength(50);
            builder.Property(p => p.ImageUrl)
                .HasMaxLength(80);

            builder.Property(p => p.UserTracksJson)
                .HasColumnName("UserTracks");
        }
    }
}