using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bakis.Models
{
    public class Contest
    {
        [Key]
        public int ContestId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Title { get; set; }
        [Required]
        public double Budget { get; set; }
        [Required]
        public DateTime PublicationDate { get; set; }
        [Required]
        public DateTime FillingDate { get; set; }
        [Required]
        public DateTime PriceRobbingDate { get; set; }
        [Required]
        public DateTime ClaimsFillingDate { get; set; }
        [ForeignKey("CustomerId")]
        public int CustomerId { get; set; }
        [ForeignKey("ContestStatusId")]
        public int ContestStatusId { get; set; }
    }
}
