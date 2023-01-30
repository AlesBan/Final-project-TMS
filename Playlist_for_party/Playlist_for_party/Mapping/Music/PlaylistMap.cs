using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Playlist_for_party.Models.Music;

namespace Playlist_for_party.Mapping.Music
{
    public class PlaylistMap : IEntityTypeConfiguration<Playlist>
    {
        public void Configure(EntityTypeBuilder<Playlist> builder)
        {
            builder.HasKey(b => b.PlaylistId);
            builder.Property(b => b.PlaylistId)
                .HasMaxLength(40);
            builder.Property(b => b.Name)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(b => b.Href)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(b => b.ImageUrl)
                .HasMaxLength(80);
            builder.Property(b => b.Duration)
                .HasDefaultValue(0);
        }
    }
}