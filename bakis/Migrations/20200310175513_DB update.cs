using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace bakis.Migrations
{
    public partial class DBupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phase",
                table: "Projects");

            migrationBuilder.CreateTable(
                name: "ContestFiles",
                columns: table => new
                {
                    ContestFileId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    File = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    ContestId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContestFiles", x => x.ContestFileId);
                });

            migrationBuilder.CreateTable(
                name: "Contests",
                columns: table => new
                {
                    ContestId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Budget = table.Column<double>(nullable: false),
                    PublicationDate = table.Column<DateTime>(nullable: false),
                    FillingDate = table.Column<DateTime>(nullable: false),
                    PriceRobbingDate = table.Column<DateTime>(nullable: false),
                    ClaimsFillingDate = table.Column<DateTime>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    ContestStatusId = table.Column<int>(nullable: false),
                    TenderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contests", x => x.ContestId);
                });

            migrationBuilder.CreateTable(
                name: "ContestStatuses",
                columns: table => new
                {
                    ContestStatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StatusName = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContestStatuses", x => x.ContestStatusId);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    CustomerTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "CustomerTypes",
                columns: table => new
                {
                    CustomerTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTypes", x => x.CustomerTypeId);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeRoles",
                columns: table => new
                {
                    EmployeeRoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    AverageWage = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeRoles", x => x.EmployeeRoleId);
                });

            migrationBuilder.CreateTable(
                name: "ProjectStageNames",
                columns: table => new
                {
                    ProjctStageNameId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StageName = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectStageNames", x => x.ProjctStageNameId);
                });

            migrationBuilder.CreateTable(
                name: "ProjectStages",
                columns: table => new
                {
                    ProjectStageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectStageNameId = table.Column<int>(nullable: false),
                    StageBudget = table.Column<double>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    ScheduledStartDate = table.Column<DateTime>(nullable: false),
                    ScheduledEndDate = table.Column<DateTime>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectStages", x => x.ProjectStageId);
                });

            migrationBuilder.CreateTable(
                name: "ResourcePlans",
                columns: table => new
                {
                    ResourcePlanId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateFrom = table.Column<DateTime>(nullable: false),
                    DateTo = table.Column<DateTime>(nullable: false),
                    Hours = table.Column<double>(nullable: false),
                    ProjectStageId = table.Column<int>(nullable: false),
                    EmployeeRoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourcePlans", x => x.ResourcePlanId);
                });

            migrationBuilder.CreateTable(
                name: "StageProgresses",
                columns: table => new
                {
                    StageProgressId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateFrom = table.Column<DateTime>(nullable: false),
                    DateTo = table.Column<DateTime>(nullable: false),
                    Percentage = table.Column<double>(nullable: false),
                    ProjectStageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StageProgresses", x => x.StageProgressId);
                });

            migrationBuilder.CreateTable(
                name: "TenderFiles",
                columns: table => new
                {
                    TenderFileId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    File = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    TenderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenderFiles", x => x.TenderFileId);
                });

            migrationBuilder.CreateTable(
                name: "Tenders",
                columns: table => new
                {
                    TenderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenderStateId = table.Column<int>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    FillingDate = table.Column<DateTime>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    ContestId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenders", x => x.TenderId);
                });

            migrationBuilder.CreateTable(
                name: "TenderStates",
                columns: table => new
                {
                    TenderStateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenderStates", x => x.TenderStateId);
                });

            migrationBuilder.CreateTable(
                name: "WorkingTimeRegisters",
                columns: table => new
                {
                    WorkingTimeRegisterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateFrom = table.Column<DateTime>(nullable: false),
                    DateTo = table.Column<DateTime>(nullable: false),
                    Hours = table.Column<double>(nullable: false),
                    ProjectStageId = table.Column<int>(nullable: false),
                    EmployeeRoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingTimeRegisters", x => x.WorkingTimeRegisterId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContestFiles");

            migrationBuilder.DropTable(
                name: "Contests");

            migrationBuilder.DropTable(
                name: "ContestStatuses");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "CustomerTypes");

            migrationBuilder.DropTable(
                name: "EmployeeRoles");

            migrationBuilder.DropTable(
                name: "ProjectStageNames");

            migrationBuilder.DropTable(
                name: "ProjectStages");

            migrationBuilder.DropTable(
                name: "ResourcePlans");

            migrationBuilder.DropTable(
                name: "StageProgresses");

            migrationBuilder.DropTable(
                name: "TenderFiles");

            migrationBuilder.DropTable(
                name: "Tenders");

            migrationBuilder.DropTable(
                name: "TenderStates");

            migrationBuilder.DropTable(
                name: "WorkingTimeRegisters");

            migrationBuilder.AddColumn<string>(
                name: "Phase",
                table: "Projects",
                type: "nvarchar(100)",
                nullable: true);
        }
    }
}
