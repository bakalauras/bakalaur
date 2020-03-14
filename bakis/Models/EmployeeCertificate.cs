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
        [Column(TypeName = "nvarchar(100)")]
        public string File { get; set; }
        [ForeignKey("EmployeeId")]
        public int EmployeeId { get; set; }
    }
}
