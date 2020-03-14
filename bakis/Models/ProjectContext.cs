using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bakis.Models
{
    public class ProjectContext : DbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {

        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Duty> Duties { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeDuty> EmployeeDuties { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Competency> Competencies { get; set; }
        public DbSet<EmployeeCompetency> EmployeeCompetencies { get; set; }
        public DbSet<StageCompetency> StageCompetencies { get; set; }
        public DbSet<EmployeeExam> EmployeeExams { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<EmployeeCertificate> EmployeeCertificates { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ContestCertificate> ContestCertificates { get; set; }

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
