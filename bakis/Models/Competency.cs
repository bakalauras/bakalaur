using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bakis.Models
{
    public class Competency
    {
        [Key]
        public int CompetencyId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Title { get; set; }
    }
}
