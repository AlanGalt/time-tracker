using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GetThingsDoneAPI.Data.Migrations.TimeEntryModified
{
    public partial class TimeEntryModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "TimeEntryDate",
                table: "TimeEntries",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeEntryDate",
                table: "TimeEntries",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
