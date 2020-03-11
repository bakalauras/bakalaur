using Microsoft.EntityFrameworkCore.Migrations;

namespace bakis.Migrations
{
    public partial class Fixes5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Tenders");

            migrationBuilder.DropColumn(
                name: "TenderId",
                table: "Contests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Tenders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenderId",
                table: "Contests",
                nullable: false,
                defaultValue: 0);
        }
    }
}
