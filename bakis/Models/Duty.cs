﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bakis.Models
{
    public class Duty
    {
        [Key]
        public int DutyId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Title { get; set; }
    }
}