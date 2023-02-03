using Microsoft.EntityFrameworkCore;
using WebApp_Data.Mapping;
using WebApp_Data.Mapping.Connections;
using WebApp_Data.Mapping.Music;
using WebApp_Data.Models;
using WebApp_Data.Models.DbConnections;
using WebApp_Data.Models.Music;

namespace Playlist_for_party.Data
{
    public class MusicContext : DbContext
    {
        public MusicContext(DbContextOptions<MusicContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Track> Songs { get; set; }
        public DbSet<PlaylistTracks> PlaylistSongs { get; set; }
        public DbSet<UserEditorPlaylists> UserEditorPlaylists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new PlaylistMap());
            modelBuilder.ApplyConfiguration(new TrackMap());
            modelBuilder.ApplyConfiguration(new PlaylistTracksMap());
            modelBuilder.ApplyConfiguration(new UserEditorPlaylistsMap());
        }
    }
}