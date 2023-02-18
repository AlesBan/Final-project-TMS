using Microsoft.EntityFrameworkCore.Migrations;

namespace Playlist_for_party.Migrations
{
    public partial class UpdateInitial6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserEditorPlaylists_Playlists_PlaylistId",
                table: "UserEditorPlaylists");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEditorPlaylists_Users_UserId",
                table: "UserEditorPlaylists");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEditorPlaylists_Playlists_PlaylistId",
                table: "UserEditorPlaylists",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "PlaylistId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserEditorPlaylists_Users_UserId",
                table: "UserEditorPlaylists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserEditorPlaylists_Playlists_PlaylistId",
                table: "UserEditorPlaylists");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEditorPlaylists_Users_UserId",
                table: "UserEditorPlaylists");

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
    }
}
