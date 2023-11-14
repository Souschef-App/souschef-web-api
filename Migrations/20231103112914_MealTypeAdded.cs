using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace souschef.server.Migrations
{
    public partial class MealTypeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "MealPlans",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MealType",
                table: "MealPlanRecipes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_MealPlans_ApplicationUserId",
                table: "MealPlans",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MealPlans_AspNetUsers_ApplicationUserId",
                table: "MealPlans",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealPlans_AspNetUsers_ApplicationUserId",
                table: "MealPlans");

            migrationBuilder.DropIndex(
                name: "IX_MealPlans_ApplicationUserId",
                table: "MealPlans");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "MealPlans");

            migrationBuilder.DropColumn(
                name: "MealType",
                table: "MealPlanRecipes");
        }
    }
}
