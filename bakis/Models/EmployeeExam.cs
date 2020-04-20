﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bakis.Models
{
    public class EmployeeExam
    {
        [Key]
        public int EmployeeExamId { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PlannedExamDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RealExamDate { get; set; }
        [Required]
        public bool IsPassed { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string File { get; set; }
        [ForeignKey("ExamId")]
        public int ExamId { get; set; }
        [ForeignKey("EmployeeId")]
        public int EmployeeId { get; set; }
        [ForeignKey("CertificateId")]
        public int CertificateId { get; set; }
    }
}
