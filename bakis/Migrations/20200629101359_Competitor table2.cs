using Microsoft.EntityFrameworkCore.Migrations;

namespace bakis.Migrations
{
    public partial class Competitortable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Competitor",
                table: "Competitor");

            migrationBuilder.RenameTable(
                name: "Competitor",
                newName: "Competitors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Competitors",
                table: "Competitors",
                column: "CompetitorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Competitors",
                table: "Competitors");

            migrationBuilder.RenameTable(
                name: "Competitors",
                newName: "Competitor");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Competitor",
                table: "Competitor",
                column: "CompetitorId");
        }
    }
}
