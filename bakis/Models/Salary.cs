﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bakis.Models
{
    public class Salary
    {
        [Key]
        public int SalaryId { get; set; }
        [Required]
        public double Staff { get; set; }
        [Required]
        public double EmployeeSalary { get; set; }
        [Required]
        public DateTime DateFrom { get; set; }
        [Required]
        public DateTime DateTo { get; set; }
        [ForeignKey("EmployeeId")]
        public int EmployeeId { get; set; }
    }
}