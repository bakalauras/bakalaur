﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using bakis.Models;

namespace bakis.Migrations
{
    [DbContext(typeof(ProjectContext))]
    partial class ProjectContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("bakis.Models.Certificate", b =>
                {
                    b.Property<int>("CertificateId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Technology")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("CertificateId");

                    b.ToTable("Certificates");
                });

            modelBuilder.Entity("bakis.Models.Competency", b =>
                {
                    b.Property<int>("CompetencyId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("CompetencyId");

                    b.ToTable("Competencies");
                });

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

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ContestId");

                    b.HasIndex("ContestStatusId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Contests");
                });

            modelBuilder.Entity("bakis.Models.ContestCertificate", b =>
                {
                    b.Property<int>("ContestCertificateId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Amount");

                    b.Property<int>("CertificateId");

                    b.Property<int>("ContestId");

                    b.HasKey("ContestCertificateId");

                    b.ToTable("ContestCertificates");
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

                    b.HasIndex("ContestId");

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

            modelBuilder.Entity("bakis.Models.CPIMeasure", b =>
                {
                    b.Property<int>("CPIMeasureId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("ActualPrice");

                    b.Property<double>("CPI");

                    b.Property<DateTime>("Date");

                    b.Property<double>("PlannedPrice");

                    b.Property<int>("ProjectStageId");

                    b.HasKey("CPIMeasureId");

                    b.HasIndex("ProjectStageId");

                    b.ToTable("CPIMeasures");
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

                    b.HasIndex("CustomerTypeId");

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

            modelBuilder.Entity("bakis.Models.Duty", b =>
                {
                    b.Property<int>("DutyId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("DutyId");

                    b.ToTable("Duties");
                });

            modelBuilder.Entity("bakis.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FkEmployeeId");

                    b.Property<int>("GroupId");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("EmployeeId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("bakis.Models.EmployeeCertificate", b =>
                {
                    b.Property<int>("EmployeeCertificateId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CertificateId");

                    b.Property<int>("EmployeeId");

                    b.Property<byte[]>("File")
                        .IsRequired();

                    b.HasKey("EmployeeCertificateId");

                    b.ToTable("EmployeeCertificates");
                });

            modelBuilder.Entity("bakis.Models.EmployeeCompetency", b =>
                {
                    b.Property<int>("EmployeeCompetencyId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompetencyId");

                    b.Property<DateTime>("DateFrom");

                    b.Property<DateTime>("DateTo");

                    b.Property<int>("EmployeeId");

                    b.HasKey("EmployeeCompetencyId");

                    b.ToTable("EmployeeCompetencies");
                });

            modelBuilder.Entity("bakis.Models.EmployeeDuty", b =>
                {
                    b.Property<int>("EmployeeDutyId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateFrom");

                    b.Property<DateTime>("DateTo");

                    b.Property<int>("DutyId");

                    b.Property<int>("EmployeeId");

                    b.HasKey("EmployeeDutyId");

                    b.ToTable("EmployeeDuties");
                });

            modelBuilder.Entity("bakis.Models.EmployeeExam", b =>
                {
                    b.Property<int>("EmployeeExamId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CertificateId");

                    b.Property<int>("EmployeeId");

                    b.Property<int>("ExamId");

                    b.Property<string>("File")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsPassed");

                    b.Property<DateTime>("PlannedExamDate");

                    b.Property<double>("Price");

                    b.Property<DateTime>("RealExamDate");

                    b.HasKey("EmployeeExamId");

                    b.ToTable("EmployeeExams");
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

            modelBuilder.Entity("bakis.Models.Exam", b =>
                {
                    b.Property<int>("ExamId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CertificateId");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ExamId");

                    b.ToTable("Exams");
                });

            modelBuilder.Entity("bakis.Models.Group", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("GroupId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("bakis.Models.GroupRight", b =>
                {
                    b.Property<int>("GroupRightId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GroupId");

                    b.Property<int>("RightId");

                    b.HasKey("GroupRightId");

                    b.ToTable("GroupRights");
                });

            modelBuilder.Entity("bakis.Models.Project", b =>
                {
                    b.Property<int>("ProjectId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Budget");

                    b.Property<string>("ContractNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("CustomerId");

                    b.Property<int>("TenderId");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ProjectId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("TenderId");

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

                    b.HasIndex("ProjectId");

                    b.HasIndex("ProjectStageNameId");

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

                    b.Property<double>("Price");

                    b.Property<int>("ProjectStageId");

                    b.HasKey("ResourcePlanId");

                    b.HasIndex("EmployeeRoleId");

                    b.HasIndex("ProjectStageId");

                    b.ToTable("ResourcePlans");
                });

            modelBuilder.Entity("bakis.Models.Right", b =>
                {
                    b.Property<int>("RightId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("RightId");

                    b.ToTable("Rights");
                });

            modelBuilder.Entity("bakis.Models.Salary", b =>
                {
                    b.Property<int>("SalaryId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateFrom");

                    b.Property<DateTime>("DateTo");

                    b.Property<int>("EmployeeId");

                    b.Property<double>("EmployeeSalary");

                    b.Property<double>("Staff");

                    b.HasKey("SalaryId");

                    b.ToTable("Salaries");
                });

            modelBuilder.Entity("bakis.Models.StageCompetency", b =>
                {
                    b.Property<int>("StageCompetencyId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Amount");

                    b.Property<int>("CompetencyId");

                    b.Property<int>("ProjectStageId");

                    b.HasKey("StageCompetencyId");

                    b.ToTable("StageCompetencies");
                });

            modelBuilder.Entity("bakis.Models.StageProgress", b =>
                {
                    b.Property<int>("StageProgressId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<double>("Percentage");

                    b.Property<int>("ProjectStageId");

                    b.Property<double>("SPI");

                    b.Property<double>("ScheduledPercentage");

                    b.HasKey("StageProgressId");

                    b.HasIndex("ProjectStageId");

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

                    b.Property<int>("TenderStateId");

                    b.HasKey("TenderId");

                    b.HasIndex("ContestId");

                    b.HasIndex("TenderStateId");

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

                    b.HasIndex("TenderId");

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

                    b.Property<int>("EmployeeId");

                    b.Property<int>("EmployeeRoleId");

                    b.Property<double>("Hours");

                    b.Property<double>("Price");

                    b.Property<int>("ProjectStageId");

                    b.HasKey("WorkingTimeRegisterId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("EmployeeRoleId");

                    b.HasIndex("ProjectStageId");

                    b.ToTable("WorkingTimeRegisters");
                });

            modelBuilder.Entity("bakis.Models.Contest", b =>
                {
                    b.HasOne("bakis.Models.ContestStatus", "ContestStatus")
                        .WithMany()
                        .HasForeignKey("ContestStatusId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("bakis.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("bakis.Models.ContestFile", b =>
                {
                    b.HasOne("bakis.Models.Contest", "Contest")
                        .WithMany()
                        .HasForeignKey("ContestId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("bakis.Models.CPIMeasure", b =>
                {
                    b.HasOne("bakis.Models.ProjectStage", "ProjectStage")
                        .WithMany()
                        .HasForeignKey("ProjectStageId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("bakis.Models.Customer", b =>
                {
                    b.HasOne("bakis.Models.CustomerType", "CustomerType")
                        .WithMany()
                        .HasForeignKey("CustomerTypeId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("bakis.Models.Project", b =>
                {
                    b.HasOne("bakis.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("bakis.Models.Tender", "Tender")
                        .WithMany()
                        .HasForeignKey("TenderId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("bakis.Models.ProjectStage", b =>
                {
                    b.HasOne("bakis.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("bakis.Models.ProjectStageName", "ProjectStageName")
                        .WithMany()
                        .HasForeignKey("ProjectStageNameId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("bakis.Models.ResourcePlan", b =>
                {
                    b.HasOne("bakis.Models.EmployeeRole", "EmployeeRole")
                        .WithMany()
                        .HasForeignKey("EmployeeRoleId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("bakis.Models.ProjectStage", "ProjectStage")
                        .WithMany()
                        .HasForeignKey("ProjectStageId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("bakis.Models.StageProgress", b =>
                {
                    b.HasOne("bakis.Models.ProjectStage", "ProjectStage")
                        .WithMany()
                        .HasForeignKey("ProjectStageId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("bakis.Models.Tender", b =>
                {
                    b.HasOne("bakis.Models.Contest", "Contest")
                        .WithMany()
                        .HasForeignKey("ContestId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("bakis.Models.TenderState", "TenderState")
                        .WithMany()
                        .HasForeignKey("TenderStateId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("bakis.Models.TenderFile", b =>
                {
                    b.HasOne("bakis.Models.Tender", "Tender")
                        .WithMany()
                        .HasForeignKey("TenderId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("bakis.Models.WorkingTimeRegister", b =>
                {
                    b.HasOne("bakis.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("bakis.Models.EmployeeRole", "EmployeeRole")
                        .WithMany()
                        .HasForeignKey("EmployeeRoleId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("bakis.Models.ProjectStage", "ProjectStage")
                        .WithMany()
                        .HasForeignKey("ProjectStageId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
