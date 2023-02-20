﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Playlist_for_party.Migrations
{
    /// <inheritdoc />
    public partial class TrackRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TracksRating",
                table: "Playlists",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TracksRating",
                table: "Playlists");
        }
    }
}
