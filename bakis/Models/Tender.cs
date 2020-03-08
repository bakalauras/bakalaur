using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bakis.Models
{
    public class Tender
    {
        [Key]
        public int TenderId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string State { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public DateTime FillingDate { get; set; }
        [ForeignKey("ProjectId")]
        public int ProjectId { get; set; }
        [ForeignKey("ContestId")]
        public int ContestId { get; set; }
    }
}
