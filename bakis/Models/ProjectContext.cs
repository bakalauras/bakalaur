using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using static bakis.Controllers.ProjectsController;
using static bakis.Controllers.UploadController;
using bakis.Models;

namespace bakis.Models
{
    [ExcludeFromCodeCoverage]
    public class ProjectContext : DbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {

        }

        public DbSet<ContestStatus> ContestStatuses { get; set; }

        public DbSet<Tender> Tenders { get; set; }

        public DbSet<TenderState> TenderStates { get; set; }

        public DbSet<CustomerType> CustomerTypes { get; set; }

        public DbSet<Customer> Customers { get; set; }

        

        public DbSet<TenderFile> TenderFiles { get; set; }

        public DbSet<Contest> Contests { get; set; }

        public DbSet<ContestFile> ContestFiles { get; set; }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Duty> Duties { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeDuty> EmployeeDuties { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Competency> Competencies { get; set; }
        public DbSet<EmployeeCompetency> EmployeeCompetencies { get; set; }
        public DbSet<StageCompetency> StageCompetencies { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<EmployeeExam> EmployeeExams { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<EmployeeCertificate> EmployeeCertificates { get; set; }
        
        public DbSet<ContestCertificate> ContestCertificates { get; set; }

        public DbSet<ProjectStageName> ProjectStageNames { get; set; }

        public DbSet<ProjectStage> ProjectStages { get; set; }

        public DbSet<CPIMeasure> CPIMeasures { get; set; }

        public DbSet<StageProgress> StageProgresses { get; set; }

        public DbSet<EmployeeRole> EmployeeRoles { get; set; }

        public DbSet<ResourcePlan> ResourcePlans { get; set; }

        public DbSet<WorkingTimeRegister> WorkingTimeRegisters { get; set; }

        public DbSet<GroupRight> GroupRights { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Competitor> Competitors { get; set; }

        public DbSet<ContestCompetitor> ContestCompetitors { get; set; }
        
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            foreach (var relationship in modelbuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelbuilder);
        }
    }
}
