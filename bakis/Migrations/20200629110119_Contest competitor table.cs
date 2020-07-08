using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace bakis.Migrations
{
    public partial class Contestcompetitortable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContestCompetitors",
                columns: table => new
                {
                    ContestCompetitorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Price = table.Column<double>(nullable: false),
                    CompetitorId = table.Column<int>(nullable: false),
                    ContestId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContestCompetitors", x => x.ContestCompetitorId);
                    table.ForeignKey(
                        name: "FK_ContestCompetitors_Competitors_CompetitorId",
                        column: x => x.CompetitorId,
                        principalTable: "Competitors",
                        principalColumn: "CompetitorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContestCompetitors_Contests_ContestId",
                        column: x => x.ContestId,
                        principalTable: "Contests",
                        principalColumn: "ContestId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContestCompetitors_CompetitorId",
                table: "ContestCompetitors",
                column: "CompetitorId");

            migrationBuilder.CreateIndex(
                name: "IX_ContestCompetitors_ContestId",
                table: "ContestCompetitors",
                column: "ContestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContestCompetitors");
        }
    }
}
