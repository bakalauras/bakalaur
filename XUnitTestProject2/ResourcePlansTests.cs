using bakis.Controllers;
using bakis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using Xunit;

namespace XUnitTestProject2
{
    public class ResourcePlansTests : TestsBase
    {
        [Theory]
        [InlineData(true, 40, 168, 40)]
        [InlineData(false, -1, 0, 40)]
        public void CanCalculatePrice(bool createEmployeeRole, double expexted, double averageWage, double hours)
        {
            var controller = new ResourcePlansController(_context);
            int EmployeeId = 0;
            if(createEmployeeRole)
            {
                EmployeeRole employeeRole = createEmployeeRoles(averageWage);
                _context.EmployeeRoles.Add(employeeRole);
                EmployeeId = 1000;
            }

            ResourcePlan resourcePlan = createResourcePlan(EmployeeId, hours);
            _context.ResourcePlans.Add(resourcePlan);
            _context.SaveChanges();
            ResourcePlan res = controller.calculatePrice(resourcePlan);
            Assert.Equal(expexted, res.Price);
        }

        public ResourcePlan createResourcePlan(int id, double hours)
        {
            ResourcePlan resourcePlan = new ResourcePlan() { EmployeeRoleId = id, Hours = hours, Price = 0 };
            return resourcePlan;
        }

        public EmployeeRole createEmployeeRoles(double averageWage)
        {
            EmployeeRole employeeRole = new EmployeeRole() { EmployeeRoleId = 1000, AverageWage = averageWage, Title = "Test" };
            return employeeRole;
        }
    }
}