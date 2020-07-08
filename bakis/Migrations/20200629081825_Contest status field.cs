using Microsoft.EntityFrameworkCore.Migrations;

namespace bakis.Migrations
{
    public partial class Conteststatusfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ContestStatuses",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ContestStatuses");
        }
    }
}
