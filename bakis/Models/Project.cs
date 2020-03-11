using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bakis.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Title { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string ContractNumber { get; set; }
        [Required]
        public double Budget { get; set; }
        [ForeignKey("CustomerId")]
        public int ContestStatusId { get; set; }
        [ForeignKey("TenderId")]
        public int TenderId { get; set; }
    }
}
