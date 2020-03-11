using Microsoft.EntityFrameworkCore.Migrations;

namespace bakis.Migrations
{
    public partial class Fixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "File",
                table: "TenderFiles",
                newName: "FileName");

            migrationBuilder.RenameColumn(
                name: "File",
                table: "ContestFiles",
                newName: "FileName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "TenderFiles",
                newName: "File");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "ContestFiles",
                newName: "File");
        }
    }
}
