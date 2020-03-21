using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bakis.Models
{
    public class WorkingTimeRegister
    {
        [Key]
        public int WorkingTimeRegisterId { get; set; }
        [Required]
        public DateTime DateFrom { get; set; }
        [Required]
        public DateTime DateTo { get; set; }
        [Required]
        public double Hours { get; set; }
        [Required]
        [ForeignKey("ProjectStageId")]
        public int ProjectStageId { get; set; }
        [Required]
        [ForeignKey("EmployeeRoleId")]
        public int EmployeeRoleId { get; set; }
        [Required]
        [ForeignKey("EmployeeId")]
        public int EmployeeId { get; set; }
    }
}
