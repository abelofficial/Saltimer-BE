using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saltimer.Api.Migrations
{
    public partial class RemovedStarttimePausetimeFromMobtimer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PausedTime",
                table: "MobTimerSession");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "MobTimerSession");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PausedTime",
                table: "MobTimerSession",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "MobTimerSession",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
