using bakis.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestProject2
{
    public abstract class TestsBase
    {
        public ProjectContext _context;
        protected TestsBase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProjectContext>();
            optionsBuilder.UseInMemoryDatabase();
           _context = new ProjectContext(optionsBuilder.Options);
        }
        
    }
}
