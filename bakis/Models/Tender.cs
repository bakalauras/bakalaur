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
    public class Tender
    {
        [Key]
        public int TenderId { get; set; }
        [ForeignKey("TenderStateId")]
        public TenderState TenderState { get; set; }
        public int TenderStateId { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FillingDate { get; set; }
        [ForeignKey("ContestId")]
        public Contest Contest { get; set; }
        public int ContestId { get; set; }
    }
}
