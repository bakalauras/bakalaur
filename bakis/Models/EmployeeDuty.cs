﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace BE.Models
{
    [ExcludeFromCodeCoverage]
    public class EmployeeDuty
    {
        [Key]
        public int EmployeeDutyId { get; set; }
        [Required]
        [Display(Name = "DateFrom")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; }
        [Display(Name = "DateTo")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? DateTo { get; set; }
        [Required]
        public double Staff { get; set; }
        [ForeignKey("DutyId")]
        public Duty Duty { get; set; }
        public int DutyId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
    }
}
