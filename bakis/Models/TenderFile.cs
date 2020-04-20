using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bakis.Models
{
    public class TenderFile
    {
        [Key]
        public int TenderFileId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Description { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string FileName { get; set; }
        [ForeignKey("TenderId")]
        public Tender Tender { get; set; }
        public int TenderId { get; set; }
    }
}
