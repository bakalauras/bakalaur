using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bakis.Models
{
    public class EmployeeCompetency
    {
        [Key]
        public int EmployeeCompetencyId { get; set; }
        [Required]
        public DateTime DateFrom { get; set; }
        [Required]
        public DateTime DateTo { get; set; }
        [ForeignKey("CompetencyId")]
        public int CompetencyId { get; set; }
        [ForeignKey("EmployeeId")]
        public int EmployeeId { get; set; }
    }
}
