using bakis.Controllers;
using bakis.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestProject2
{
    public class CPIMeasuresTests
    {
        [Theory]
        [InlineData(1, "2019-05-13", 50, 50,"2019-05-01", "2019-05-13",  "2019-05-13")]
        [InlineData(1, "2020-03-03", 100, 50, "2020-03-02", "2020-03-05", "2020-03-03")]
        [InlineData(-1, "2019-05-11", 0, 50,"2019-05-13",  "2019-05-14", "2019-05-14")]
        public void CanCalculateCPI(double expexted, string date, double RPPrice, double WTPrice, string RPDateFrom, string RPDateTo,
             string WTDateTo)
        {
            using (var context = new ProjectContext(CreateNewContextOptions()))
            {
                var controller = new CPIMeasuresController(context);
                int ProjectStageId = 1;
                CPIMeasure cPIMeasure = createCPIMeasure(ProjectStageId, date);
                context.CPIMeasures.Add(cPIMeasure);
                WorkingTimeRegister workingTimeRegister = createRegister(ProjectStageId, WTPrice, WTDateTo);
                context.WorkingTimeRegisters.Add(workingTimeRegister);
                ResourcePlan resourcePlan = createResourcePlan(ProjectStageId, RPPrice, RPDateTo, RPDateFrom);
                context.ResourcePlans.Add(resourcePlan);
                context.SaveChanges();
                CPIMeasure res = controller.calculateCPI(cPIMeasure);
                Assert.Equal(expexted, res.CPI);
                // Do all of your data access and assertions in here
            }
            
        }

        public WorkingTimeRegister createRegister(int id, double price, string date)
        {
            var dateTo = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            WorkingTimeRegister workingTimeRegister = new WorkingTimeRegister() { ProjectStageId = id, Price = price, DateTo = dateTo };
            return workingTimeRegister;
        }

        public ResourcePlan createResourcePlan(int id, double price, string date, string from)
        {
            var dateTo = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            var dateFrom = DateTime.ParseExact(from, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            ResourcePlan resourcePlan = new ResourcePlan() { ProjectStageId = id, Price = price, DateTo = dateTo, DateFrom = dateFrom };
            return resourcePlan;
        }

        public CPIMeasure createCPIMeasure(int id, string date)
        {
            var dateTo = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            CPIMeasure cPIMeasure = new CPIMeasure() { ProjectStageId = id, Date = dateTo };
            return cPIMeasure;
        }

        private static DbContextOptions<ProjectContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<ProjectContext>();
            builder.UseInMemoryDatabase()
                .UseInternalServiceProvider(serviceProvider);
            //var optionsBuilder = new DbContextOptionsBuilder<ProjectContext>();
            // optionsBuilder.UseInMemoryDatabase();
            // _context = new ProjectContext(optionsBuilder.Options);
            return builder.Options;
        }
    }
}
