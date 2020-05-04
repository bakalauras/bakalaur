using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bakis.Models
{
    public class ContestCertificate
    {
        [Key]
        public int ContestCertificateId { get; set; }
        [Required]
        public int Amount { get; set; }
        [ForeignKey("CertificateId")]
        public Certificate Certificate { get; set; }
        public int CertificateId { get; set; }
        [ForeignKey("ContestId")]
        public Contest Contest { get; set; }
        public int ContestId { get; set; }
    }
}
