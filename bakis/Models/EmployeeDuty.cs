using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bakis.Models
{
    public class EmployeeDuty
    {
        [Key]
        public int EmployeeDutyId { get; set; }
        [Required]
        public DateTime DateFrom { get; set; }
        [Required]
        public DateTime DateTo { get; set; }
        [ForeignKey("DutyId")]
        public int DutyId { get; set; }
        [ForeignKey("EmployeeId")]
        public int EmployeeId { get; set; }
    }
}
