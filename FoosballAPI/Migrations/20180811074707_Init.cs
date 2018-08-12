using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FoosballAPI.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Teams",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    FirstTeamId = table.Column<Guid>(nullable: true),
                    SecondTeamId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Teams_FirstTeamId",
                        column: x => x.FirstTeamId,
                        principalSchema: "dbo",
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_Teams_SecondTeamId",
                        column: x => x.SecondTeamId,
                        principalSchema: "dbo",
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sets",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: false),
                    FirstTeamResult = table.Column<int>(nullable: false),
                    SecondTeamResult = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sets", x => new { x.Id, x.ParentId });
                    table.ForeignKey(
                        name: "FK_Sets_Games_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "dbo",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_FirstTeamId",
                schema: "dbo",
                table: "Games",
                column: "FirstTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_SecondTeamId",
                schema: "dbo",
                table: "Games",
                column: "SecondTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_ParentId",
                schema: "dbo",
                table: "Sets",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sets",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Games",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Teams",
                schema: "dbo");
        }
    }
}
