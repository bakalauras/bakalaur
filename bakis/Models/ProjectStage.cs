using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bakis.Models
{
    public class ProjectStage
    {
        [Key]
        public int ProjectStageId { get; set; }
        [ForeignKey("ProjectStageNameId")]
        public int ProjectStageNameId { get; set; }
        [Required]
        public double StageBudget { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public DateTime ScheduledStartDate { get; set; }
        [Required]
        public DateTime ScheduledEndDate { get; set; }
        [Required]
        [ForeignKey("ProjectId")]
        public int ProjectId { get; set; }
    }
}
