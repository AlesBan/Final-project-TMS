using Microsoft.EntityFrameworkCore;
using Playlist_for_party.Mapping;
using Playlist_for_party.Mapping.Connections;
using Playlist_for_party.Models;
using Playlist_for_party.Models.Connections;
using Playlist_for_party.Models.Music;

namespace Playlist_for_party.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<PlaylistSongs> PlaylistSongs { get; set; }
        public DbSet<UserEditorPlaylists> UserEditorPlaylists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new PlaylistMap());
            modelBuilder.ApplyConfiguration(new SongMap());
            modelBuilder.ApplyConfiguration(new PlaylistSongsMap());
            modelBuilder.ApplyConfiguration(new UserEditorPlaylistsMap());
        }
        
    }
}