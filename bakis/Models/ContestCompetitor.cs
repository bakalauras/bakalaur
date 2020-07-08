using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bakis.Models
{
    public class ContestCompetitor
    {
        [Key]
        public int ContestCompetitorId { get; set; }
        [Required]
        public double Price { get; set; }
        [ForeignKey("CompetitorId")]
        public Competitor Competitor { get; set; }
        public int CompetitorId { get; set; }
        [ForeignKey("ContestId")]
        public Contest Contest { get; set; }
        public int ContestId { get; set; }
    }
}
