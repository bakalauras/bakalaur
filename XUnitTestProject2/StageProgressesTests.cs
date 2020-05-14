using bakis.Controllers;
using bakis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace XUnitTestProject2
{
    public class StageProgressesTests : TestsBase
    {
        [Theory]
        [InlineData(true, 1, 50, "2020-05-11", "2020-05-14", "2020-05-13", "2020-05-14")]
        [InlineData(true, -1, 50, "2019-10-10", "2020-10-10", "2020-10-10", "2020-10-10")]
        [InlineData(false, -1, 50, "2019-10-10", "2019-10-10", "2019-10-10", "2019-10-10")]
        public void CanCalculateSPI(bool createProjectStage, double expexted, double percentage, string scheduledStartDate, string scheduledEndDate, string startDate, string date)
        {
            DateTime x = new DateTime(2020,10,10);
            var controller = new StageProgressesController(_context);
            int ProjectStageId = 0;
            if (createProjectStage)
            {
                ProjectStage projectStage = createProjectStages(scheduledStartDate, scheduledEndDate, startDate);
                _context.ProjectStages.Add(projectStage);
                ProjectStageId = projectStage.ProjectStageId;
            }
            StageProgress stageProgress = createStageProgress(ProjectStageId, percentage, date);
            _context.StageProgresses.Add(stageProgress);
            _context.SaveChanges();
            StageProgress res = controller.calculateSPI(stageProgress);
            Assert.Equal(expexted, res.SPI);
        }

        public StageProgress createStageProgress(int id, double percentage, string date)
        {
            var dateTo = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            StageProgress stageProgress = new StageProgress() { ProjectStageId = id, Percentage = percentage, Date = dateTo };
            return stageProgress;
        }

        public ProjectStage createProjectStages(string scheduledStartDate, string scheduledEndDate, string startDate)
        {
            var scheduledStartDate1 = DateTime.ParseExact(scheduledStartDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            var scheduledEndDate1 = DateTime.ParseExact(scheduledEndDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            var startDate1 = DateTime.ParseExact(startDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            ProjectStage projectStage = new ProjectStage() { ScheduledStartDate = scheduledStartDate1, ScheduledEndDate = scheduledEndDate1, StartDate = startDate1 };
            return projectStage;
        }
    }
}
