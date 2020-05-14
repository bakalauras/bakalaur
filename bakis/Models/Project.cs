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
        public Customer Customer { get; set; }
        public int  CustomerId{ get; set; }
        [ForeignKey("TenderId")]
        public Tender Tender { get; set; }
        public int TenderId { get; set; }
    }
}