using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saltimer.Api.Migrations
{
    public partial class scaledDownMobtimer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BreakTime",
                table: "MobTimerSession");

            migrationBuilder.DropColumn(
                name: "PausedTime",
                table: "MobTimerSession");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BreakTime",
                table: "MobTimerSession",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PausedTime",
                table: "MobTimerSession",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
