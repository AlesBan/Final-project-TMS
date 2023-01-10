﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Playlist_for_party.Data;

namespace Playlist_for_party.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20230107134655_AddModels")]
    partial class AddModels
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Playlist_for_party.Models.Connections.PlaylistSongs", b =>
                {
                    b.Property<Guid>("PlaylistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SongId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PlaylistId", "SongId");

                    b.HasIndex("SongId");

                    b.ToTable("PlaylistSongs");
                });

            modelBuilder.Entity("Playlist_for_party.Models.Connections.UserEditorPlaylists", b =>
                {
                    b.Property<Guid>("PlaylistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserEditorId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PlaylistId", "UserEditorId");

                    b.HasIndex("UserEditorId");

                    b.ToTable("UserEditorPlaylists");
                });

            modelBuilder.Entity("Playlist_for_party.Models.Music.Album", b =>
                {
                    b.Property<Guid>("AlbumId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasMaxLength(36);

                    b.Property<Guid?>("ArtistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Duration")
                        .HasColumnType("float");

                    b.Property<string>("ImageRef")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("AlbumId");

                    b.HasIndex("ArtistId");

                    b.ToTable("Albums");
                });

            modelBuilder.Entity("Playlist_for_party.Models.Music.Artist", b =>
                {
                    b.Property<Guid>("ArtistId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasMaxLength(36);

                    b.Property<string>("ArtistName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("ImageRef")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("ArtistId");

                    b.ToTable("Artists");
                });

            modelBuilder.Entity("Playlist_for_party.Models.Music.Playlist", b =>
                {
                    b.Property<Guid>("PlaylistId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasMaxLength(40);

                    b.Property<double>("Duration")
                        .HasColumnType("float");

                    b.Property<string>("ImageRef")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("OwnerUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("UrlRef")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("PlaylistId");

                    b.HasIndex("OwnerUserId");

                    b.ToTable("Playlists");
                });

            modelBuilder.Entity("Playlist_for_party.Models.Music.Song", b =>
                {
                    b.Property<Guid>("SongId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasMaxLength(36);

                    b.Property<Guid>("AlbumId")
                        .HasColumnType("uniqueidentifier")
                        .HasMaxLength(40);

                    b.Property<Guid>("ArtistId")
                        .HasColumnType("uniqueidentifier")
                        .HasMaxLength(40);

                    b.Property<double>("Duration")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("float")
                        .HasDefaultValue(0.0);

                    b.Property<string>("ImageRef")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("Popularity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("SongId");

                    b.HasIndex("AlbumId");

                    b.HasIndex("ArtistId");

                    b.ToTable("Songs");
                });

            modelBuilder.Entity("Playlist_for_party.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasMaxLength(40);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("ImageRef")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Playlist_for_party.Models.Connections.PlaylistSongs", b =>
                {
                    b.HasOne("Playlist_for_party.Models.Music.Playlist", "Playlist")
                        .WithMany("PlaylistSongs")
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Playlist_for_party.Models.Music.Song", "Song")
                        .WithMany("PlaylistSongs")
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Playlist_for_party.Models.Connections.UserEditorPlaylists", b =>
                {
                    b.HasOne("Playlist_for_party.Models.Music.Playlist", "Playlist")
                        .WithMany("UserEditorPlaylists")
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Playlist_for_party.Models.User", "UserEditor")
                        .WithMany("UserEditorPlaylists")
                        .HasForeignKey("UserEditorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Playlist_for_party.Models.Music.Album", b =>
                {
                    b.HasOne("Playlist_for_party.Models.Music.Artist", "Artist")
                        .WithMany("Albums")
                        .HasForeignKey("ArtistId");
                });

            modelBuilder.Entity("Playlist_for_party.Models.Music.Playlist", b =>
                {
                    b.HasOne("Playlist_for_party.Models.User", "Owner")
                        .WithMany("CreatedPlaylists")
                        .HasForeignKey("OwnerUserId");
                });

            modelBuilder.Entity("Playlist_for_party.Models.Music.Song", b =>
                {
                    b.HasOne("Playlist_for_party.Models.Music.Album", "Album")
                        .WithMany("Songs")
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Playlist_for_party.Models.Music.Artist", "Artist")
                        .WithMany("Songs")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}