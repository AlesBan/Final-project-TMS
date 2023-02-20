﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Playlist_for_party.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Tracks",
                columns: table => new
                {
                    TrackId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    ArtistName = table.Column<string>(maxLength: 50, nullable: false),
                    Album = table.Column<string>(maxLength: 50, nullable: false),
                    Rating = table.Column<int>(nullable: false, defaultValue: 0),
                    DurationMs = table.Column<double>(nullable: false, defaultValue: 0.0),
                    ImageUrl = table.Column<string>(maxLength: 80, nullable: false),
                    Href = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracks", x => x.TrackId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    UserName = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    ImageRef = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    PlaylistId = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    OwnerId = table.Column<Guid>(nullable: false),
                    Href = table.Column<string>(maxLength: 50, nullable: true),
                    ImageUrl = table.Column<string>(maxLength: 80, nullable: true),
                    UserTracks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlists", x => x.PlaylistId);
                    table.ForeignKey(
                        name: "FK_Playlists_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRole_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistTracks",
                columns: table => new
                {
                    PlaylistId = table.Column<Guid>(nullable: false),
                    TrackId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistTracks", x => new { x.PlaylistId, x.TrackId });
                    table.ForeignKey(
                        name: "FK_PlaylistTracks_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "PlaylistId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaylistTracks_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "TrackId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserEditorPlaylists",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    PlaylistId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEditorPlaylists", x => new { x.PlaylistId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserEditorPlaylists_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "PlaylistId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserEditorPlaylists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_OwnerId",
                table: "Playlists",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistTracks_TrackId",
                table: "PlaylistTracks",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEditorPlaylists_UserId",
                table: "UserEditorPlaylists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaylistTracks");

            migrationBuilder.DropTable(
                name: "UserEditorPlaylists");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Tracks");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
