using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace souschef.server.Migrations
{
    public partial class TaskDepGUID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid[]>(
                name: "Dependencies",
                table: "Tasks",
                type: "uuid[]",
                nullable: true,
                oldClrType: typeof(int[]),
                oldType: "integer[]",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int[]>(
                name: "Dependencies",
                table: "Tasks",
                type: "integer[]",
                nullable: true,
                oldClrType: typeof(Guid[]),
                oldType: "uuid[]",
                oldNullable: true);
        }
    }
}
