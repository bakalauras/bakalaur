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
    public class CPIMeasure
    {
        [Key]
        public int CPIMeasureId { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public double PlannedPrice { get; set; }
        public double ActualPrice { get; set; }
        public double CPI { get; set; }
        [ForeignKey("ProjectStageId")]
        public ProjectStage ProjectStage { get; set; }
        public int ProjectStageId { get; set; }
    }
}
