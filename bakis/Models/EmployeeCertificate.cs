using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bakis.Models
{
    public class EmployeeCertificate
    {
        [Key]
        public int EmployeeCertificateId { get; set; }
        [Required]
        public byte[] File { get; set; }
        [ForeignKey("EmployeeId")]
        public int EmployeeId { get; set; }
        [ForeignKey("CertificateId")]
        public int CertificateId { get; set; }
    }
}
