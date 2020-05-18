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
    public class EmployeeCompetency
    {
        [Key]
        public int EmployeeCompetencyId { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateFrom { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateTo { get; set; }
        [ForeignKey("CompetencyId")]
        public Competency Competency { get; set; }
        public int CompetencyId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
    }
}
