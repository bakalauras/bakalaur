using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bakis.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Title { get; set; }
    }
}
