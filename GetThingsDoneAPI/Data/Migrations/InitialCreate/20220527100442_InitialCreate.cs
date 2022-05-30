using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GetThingsDoneAPI.Data.Migrations.InitialCreate
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectName = table.Column<string>(type: "text", nullable: false),
                    ProjectCode = table.Column<string>(type: "text", nullable: false),
                    ProjectActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectId);
                });

            migrationBuilder.CreateTable(
                name: "Issues",
                columns: table => new
                {
                    IssueId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IssueName = table.Column<string>(type: "text", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    IssueActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issues", x => x.IssueId);
                    table.ForeignKey(
                        name: "FK_Issues_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimeEntries",
                columns: table => new
                {
                    TimeEntryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TimeEntryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TimeEntryHours = table.Column<int>(type: "integer", nullable: false),
                    TimeEntryDescription = table.Column<string>(type: "text", nullable: false),
                    IssueId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeEntries", x => x.TimeEntryId);
                    table.ForeignKey(
                        name: "FK_TimeEntries_Issues_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issues",
                        principalColumn: "IssueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Issues_ProjectId",
                table: "Issues",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_IssueId",
                table: "TimeEntries",
                column: "IssueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeEntries");

            migrationBuilder.DropTable(
                name: "Issues");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
