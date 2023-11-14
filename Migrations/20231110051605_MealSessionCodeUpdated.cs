using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace souschef.server.Migrations
{
    public partial class MealSessionCodeUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ServerIp",
                table: "MealSessions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SessionCode",
                table: "MealSessions",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServerIp",
                table: "MealSessions");

            migrationBuilder.DropColumn(
                name: "SessionCode",
                table: "MealSessions");
        }
    }
}
