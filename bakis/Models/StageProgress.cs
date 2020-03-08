using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bakis.Models
{
    public class StageProgress
    {
        [Key]
        public int StageProgressId { get; set; }
        [Required]
        public DateTime DateFrom { get; set; }
        [Required]
        public DateTime DateTo { get; set; }
        [Required]
        public double Percentage { get; set; }
        [Required]
        [ForeignKey("ProjectStageId")]
        public int ProjectStageId { get; set; }
    }
}
