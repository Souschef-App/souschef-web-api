using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace souschef.server.Migrations
{
    public partial class RenamedLiveSession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CookingSessions",
                table: "CookingSessions");

            migrationBuilder.RenameTable(
                name: "CookingSessions",
                newName: "LiveSessions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LiveSessions",
                table: "LiveSessions",
                column: "Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LiveSessions",
                table: "LiveSessions");

            migrationBuilder.RenameTable(
                name: "LiveSessions",
                newName: "CookingSessions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CookingSessions",
                table: "CookingSessions",
                column: "Code");
        }
    }
}
