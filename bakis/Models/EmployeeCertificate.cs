﻿using System;
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
        public string File { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
        [ForeignKey("CertificateId")]
        public Certificate Certificate { get; set; }
        public int CertificateId { get; set; }
    }
}
