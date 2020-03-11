using Microsoft.EntityFrameworkCore.Migrations;

namespace bakis.Migrations
{
    public partial class Fixes2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TenderStateId",
                table: "Tenders",
                newName: "TenderState");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TenderState",
                table: "Tenders",
                newName: "TenderStateId");
        }
    }
}
