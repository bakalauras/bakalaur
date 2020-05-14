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
    public class ProjectStageName
    {
        [Key]
        public int ProjctStageNameId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string StageName { get; set; }
    }
}
