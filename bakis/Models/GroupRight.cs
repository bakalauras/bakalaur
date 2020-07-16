using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace BE.Models
{
    [ExcludeFromCodeCoverage]
    public class GroupRight
    {
        [Key]
        public int GroupRightId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Title { get; set; }
        [Required]
        public bool manageClassifiers { get; set; }
        [Required]
        public bool manageContests { get; set; }
        [Required]
        public bool manageTenders { get; set; }
        [Required]
        public bool manageProjects { get; set; }
        [Required]
        public bool manageEmployees { get; set; }
        [Required]
        public bool manageUsers{ get; set; }
        [Required]
        public bool manageCustomers { get; set; }
    }
}
