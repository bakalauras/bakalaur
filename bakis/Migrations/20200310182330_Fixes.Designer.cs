﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using bakis.Models;

namespace bakis.Migrations
{
    [DbContext(typeof(ProjectContext))]
    [Migration("20200310182330_Fixes")]
    partial class Fixes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("bakis.Models.Contest", b =>
                {
                    b.Property<int>("ContestId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Budget");

                    b.Property<DateTime>("ClaimsFillingDate");

                    b.Property<int>("ContestStatusId");

                    b.Property<int>("CustomerId");

                    b.Property<DateTime>("FillingDate");

                    b.Property<DateTime>("PriceRobbingDate");

                    b.Property<DateTime>("PublicationDate");

                    b.Property<int>("TenderId");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ContestId");

                    b.ToTable("Contests");
                });

            modelBuilder.Entity("bakis.Models.ContestFile", b =>
                {
                    b.Property<int>("ContestFileId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ContestId");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ContestFileId");

                    b.ToTable("ContestFiles");
                });

            modelBuilder.Entity("bakis.Models.ContestStatus", b =>
                {
                    b.Property<int>("ContestStatusId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("StatusName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ContestStatusId");

                    b.ToTable("ContestStatuses");
                });

            modelBuilder.Entity("bakis.Models.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Adress")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("CustomerTypeId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("CustomerId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("bakis.Models.CustomerType", b =>
                {
                    b.Property<int>("CustomerTypeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("CustomerTypeId");

                    b.ToTable("CustomerTypes");
                });

            modelBuilder.Entity("bakis.Models.EmployeeRole", b =>
                {
                    b.Property<int>("EmployeeRoleId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("AverageWage");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("EmployeeRoleId");

                    b.ToTable("EmployeeRoles");
                });

            modelBuilder.Entity("bakis.Models.Project", b =>
                {
                    b.Property<int>("ProjectId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Budget");

                    b.Property<int>("ContestStatusId");

                    b.Property<string>("ContractNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("TenderId");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ProjectId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("bakis.Models.ProjectStage", b =>
                {
                    b.Property<int>("ProjectStageId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EndDate");

                    b.Property<int>("ProjectId");

                    b.Property<int>("ProjectStageNameId");

                    b.Property<DateTime>("ScheduledEndDate");

                    b.Property<DateTime>("ScheduledStartDate");

                    b.Property<double>("StageBudget");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("ProjectStageId");

                    b.ToTable("ProjectStages");
                });

            modelBuilder.Entity("bakis.Models.ProjectStageName", b =>
                {
                    b.Property<int>("ProjctStageNameId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("StageName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ProjctStageNameId");

                    b.ToTable("ProjectStageNames");
                });

            modelBuilder.Entity("bakis.Models.ResourcePlan", b =>
                {
                    b.Property<int>("ResourcePlanId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateFrom");

                    b.Property<DateTime>("DateTo");

                    b.Property<int>("EmployeeRoleId");

                    b.Property<double>("Hours");

                    b.Property<int>("ProjectStageId");

                    b.HasKey("ResourcePlanId");

                    b.ToTable("ResourcePlans");
                });

            modelBuilder.Entity("bakis.Models.StageProgress", b =>
                {
                    b.Property<int>("StageProgressId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateFrom");

                    b.Property<DateTime>("DateTo");

                    b.Property<double>("Percentage");

                    b.Property<int>("ProjectStageId");

                    b.HasKey("StageProgressId");

                    b.ToTable("StageProgresses");
                });

            modelBuilder.Entity("bakis.Models.Tender", b =>
                {
                    b.Property<int>("TenderId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ContestId");

                    b.Property<DateTime>("FillingDate");

                    b.Property<double>("Price");

                    b.Property<int>("ProjectId");

                    b.Property<int>("TenderStateId");

                    b.HasKey("TenderId");

                    b.ToTable("Tenders");
                });

            modelBuilder.Entity("bakis.Models.TenderFile", b =>
                {
                    b.Property<int>("TenderFileId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("TenderId");

                    b.HasKey("TenderFileId");

                    b.ToTable("TenderFiles");
                });

            modelBuilder.Entity("bakis.Models.TenderState", b =>
                {
                    b.Property<int>("TenderStateId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("TenderStateId");

                    b.ToTable("TenderStates");
                });

            modelBuilder.Entity("bakis.Models.WorkingTimeRegister", b =>
                {
                    b.Property<int>("WorkingTimeRegisterId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateFrom");

                    b.Property<DateTime>("DateTo");

                    b.Property<int>("EmployeeRoleId");

                    b.Property<double>("Hours");

                    b.Property<int>("ProjectStageId");

                    b.HasKey("WorkingTimeRegisterId");

                    b.ToTable("WorkingTimeRegisters");
                });
#pragma warning restore 612, 618
        }
    }
}
