using Microsoft.EntityFrameworkCore;
using WebApp_Data.Mapping.ConnectionsMapping;
using WebApp_Data.Mapping.MusicMapping;
using WebApp_Data.Mapping.UserMapping;
using WebApp_Data.Models.DbConnections;
using WebApp_Data.Models.Music;
using WebApp_Data.Models.UserData;

namespace Playlist_for_party.Data
{
    public class MusicContext : DbContext
    {
        public MusicContext(DbContextOptions<MusicContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<PlaylistTrack> PlaylistTracks { get; set; }
        public DbSet<UserEditorPlaylist> UserEditorPlaylists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new RoleMap());
            modelBuilder.ApplyConfiguration(new UserRolesMap());
            modelBuilder.ApplyConfiguration(new PlaylistMap());
            modelBuilder.ApplyConfiguration(new TrackMap());
            modelBuilder.ApplyConfiguration(new PlaylistTrackMap());
            modelBuilder.ApplyConfiguration(new UserEditorPlaylistMap());
        }
    }
}