using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace BE.Models
{
    [ExcludeFromCodeCoverage]
    public class StageCompetency
    {
        [Key]
        public int StageCompetencyId { get; set; }
        [Required]
        public int Amount { get; set; }
        [ForeignKey("CompetencyId")]
        public Competency Competency { get; set; }
        public int CompetencyId { get; set; }
        [ForeignKey("ProjectStageId")]
        public ProjectStage ProjectStage { get; set; }
        public int ProjectStageId { get; set; }
    }
}
