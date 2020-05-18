using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace bakis.Models
{
    [ExcludeFromCodeCoverage]
    public class ProjectStage
    {
        [Key]
        public int ProjectStageId { get; set; }
        [ForeignKey("ProjectStageNameId")]
        public ProjectStageName ProjectStageName { get; set; }
        public int ProjectStageNameId { get; set; }
        [Required]
        public double StageBudget { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{yyyy'-'MM'-'dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ScheduledStartDate { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ScheduledEndDate { get; set; }        
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
        public int ProjectId { get; set; }
    }
}
