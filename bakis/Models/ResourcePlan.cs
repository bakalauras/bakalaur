using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bakis.Models
{
    public class ResourcePlan
    {
        [Key]
        public int ResourcePlanId { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateFrom { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateTo { get; set; }
        [Required]
        public double Hours { get; set; }
        public double Price { get; set; }
        [ForeignKey("ProjectStageId")]
        public ProjectStage ProjectStage { get; set; }
        public int ProjectStageId { get; set; }
        [ForeignKey("EmployeeRoleId")]
        public EmployeeRole EmployeeRole { get; set; }
        public int EmployeeRoleId { get; set; }
    }
}
