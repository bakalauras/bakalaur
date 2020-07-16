using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace BE.Models
{
    [ExcludeFromCodeCoverage]
    public class CustomerType
    {
        [Key]
        public int CustomerTypeId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Type { get; set; }
    }
}
