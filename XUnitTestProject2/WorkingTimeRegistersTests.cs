using bakis.Controllers;
using bakis.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestProject2
{
    public class WorkingTimeRegistersTests : TestsBase
    {
        [Theory]
        [InlineData(true, 80, 168, 40, 0.5)]
        [InlineData(false, -1, 0, 40, 0)]
        public void CanCalculatePrice(bool createEmployeeSalary, double expexted, double employeeSalary, double hours, double staff)
        {
            var controller = new WorkingTimeRegistersController(_context);
            int EmployeeId = 0;
            if (createEmployeeSalary)
            {
                Salary salary = createSalary(employeeSalary, staff);
                _context.Salaries.Add(salary);
                EmployeeId = 1000;
            }
            WorkingTimeRegister workingTimeRegister = createRegister(EmployeeId, hours);
            _context.WorkingTimeRegisters.Add(workingTimeRegister);
            _context.SaveChanges();
            WorkingTimeRegister res = controller.calculatePrice(workingTimeRegister);
            Assert.Equal(expexted, res.Price);
        }

        public WorkingTimeRegister createRegister(int id, double hours)
        {
            var dateTo = new DateTime(2020, 04, 10);
            WorkingTimeRegister workingTimeRegister = new WorkingTimeRegister() { EmployeeId = id, Hours = hours, Price = 0, DateTo = dateTo };
            return workingTimeRegister;
        }

        public Salary createSalary(double employeeSalary, double staff)
        {
            var dateFrom = new DateTime(2019, 10, 10);
            var dateTo = new DateTime(2020, 10, 10);
            Salary salary = new Salary() { EmployeeId = 1000, DateFrom = dateFrom, DateTo = dateTo, EmployeeSalary = employeeSalary, Staff = staff };
            return salary;
        }
    }
}
