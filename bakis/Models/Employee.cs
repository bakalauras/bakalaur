using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace bakis.Models
{
    [ExcludeFromCodeCoverage]
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Surname { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [ForeignKey("EmployeeId")]
        public int? FkEmployeeId { get; set; }
    }
}
