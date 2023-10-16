using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace souschef.server.Migrations
{
    public partial class CookingSession_Updated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_CookingSession_CookingSessionId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CookingSession_AspNetUsers_HostId",
                table: "CookingSession");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_CookingSession_CookingSessionId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_CookingSessionId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CookingSessionId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CookingSession",
                table: "CookingSession");

            migrationBuilder.DropIndex(
                name: "IX_CookingSession_HostId",
                table: "CookingSession");

            migrationBuilder.DropColumn(
                name: "CookingSessionId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "CookingSessionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CurrentSessionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CookingSession");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "CookingSession");

            migrationBuilder.DropColumn(
                name: "HostId",
                table: "CookingSession");

            migrationBuilder.DropColumn(
                name: "OccasionType",
                table: "CookingSession");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "CookingSession");

            migrationBuilder.RenameTable(
                name: "CookingSession",
                newName: "CookingSessions");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "CookingSessions",
                newName: "IP");

            migrationBuilder.AddColumn<int>(
                name: "Code",
                table: "CookingSessions",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CookingSessions",
                table: "CookingSessions",
                column: "Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CookingSessions",
                table: "CookingSessions");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "CookingSessions");

            migrationBuilder.RenameTable(
                name: "CookingSessions",
                newName: "CookingSession");

            migrationBuilder.RenameColumn(
                name: "IP",
                table: "CookingSession",
                newName: "Name");

            migrationBuilder.AddColumn<Guid>(
                name: "CookingSessionId",
                table: "Recipes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CookingSessionId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentSessionId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "CookingSession",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<long>(
                name: "Date",
                table: "CookingSession",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HostId",
                table: "CookingSession",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OccasionType",
                table: "CookingSession",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "CookingSession",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_CookingSession",
                table: "CookingSession",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_CookingSessionId",
                table: "Recipes",
                column: "CookingSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CookingSessionId",
                table: "AspNetUsers",
                column: "CookingSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_CookingSession_HostId",
                table: "CookingSession",
                column: "HostId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_CookingSession_CookingSessionId",
                table: "AspNetUsers",
                column: "CookingSessionId",
                principalTable: "CookingSession",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CookingSession_AspNetUsers_HostId",
                table: "CookingSession",
                column: "HostId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_CookingSession_CookingSessionId",
                table: "Recipes",
                column: "CookingSessionId",
                principalTable: "CookingSession",
                principalColumn: "Id");
        }
    }
}
