using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bakis.Models
{
    public class ContestStatus
    {
        [Key]
        public int ContestStatusId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string StatusName { get; set; }
    }
}
