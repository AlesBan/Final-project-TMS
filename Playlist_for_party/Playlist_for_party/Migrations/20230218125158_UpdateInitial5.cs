using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Playlist_for_party.Migrations
{
    public partial class UpdateInitial5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserEditorPlaylists_Playlists_PlaylistId",
                table: "UserEditorPlaylists");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEditorPlaylists_Users_UserEditorId",
                table: "UserEditorPlaylists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserEditorPlaylists",
                table: "UserEditorPlaylists");

            migrationBuilder.DropIndex(
                name: "IX_UserEditorPlaylists_UserEditorId",
                table: "UserEditorPlaylists");

            migrationBuilder.DropColumn(
                name: "UserEditorId",
                table: "UserEditorPlaylists");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "UserEditorPlaylists",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserEditorPlaylists",
                table: "UserEditorPlaylists",
                columns: new[] { "PlaylistId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserEditorPlaylists_UserId",
                table: "UserEditorPlaylists",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEditorPlaylists_Playlists_PlaylistId",
                table: "UserEditorPlaylists",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "PlaylistId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserEditorPlaylists_Users_UserId",
                table: "UserEditorPlaylists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserEditorPlaylists_Playlists_PlaylistId",
                table: "UserEditorPlaylists");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEditorPlaylists_Users_UserId",
                table: "UserEditorPlaylists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserEditorPlaylists",
                table: "UserEditorPlaylists");

            migrationBuilder.DropIndex(
                name: "IX_UserEditorPlaylists_UserId",
                table: "UserEditorPlaylists");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserEditorPlaylists");

            migrationBuilder.AddColumn<Guid>(
                name: "UserEditorId",
                table: "UserEditorPlaylists",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserEditorPlaylists",
                table: "UserEditorPlaylists",
                columns: new[] { "PlaylistId", "UserEditorId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserEditorPlaylists_UserEditorId",
                table: "UserEditorPlaylists",
                column: "UserEditorId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEditorPlaylists_Playlists_PlaylistId",
                table: "UserEditorPlaylists",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "PlaylistId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserEditorPlaylists_Users_UserEditorId",
                table: "UserEditorPlaylists",
                column: "UserEditorId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
