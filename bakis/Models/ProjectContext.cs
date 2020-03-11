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

        public DbSet<ContestStatus> ContestStatuses { get; set; }

        public DbSet<Tender> Tenders { get; set; }

        public DbSet<TenderState> TenderStates { get; set; }

        public DbSet<CustomerType> CustomerTypes { get; set; }

        public DbSet<Customer> Customers { get; set; }

        

        public DbSet<TenderFile> TenderFiles { get; set; }

        public DbSet<Contest> Contests { get; set; }

        public DbSet<ContestFile> ContestFiles { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<ProjectStageName> ProjectStageNames { get; set; }

        public DbSet<ProjectStage> ProjectStages { get; set; }

        public DbSet<StageProgress> StageProgresses { get; set; }

        public DbSet<EmployeeRole> EmployeeRoles { get; set; }

        public DbSet<ResourcePlan> ResourcePlans { get; set; }

        public DbSet<WorkingTimeRegister> WorkingTimeRegisters { get; set; }

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
