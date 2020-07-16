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
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [ForeignKey("GroupRightId")]
        public GroupRight GroupRight { get; set; }
        public int GroupRightId { get; set; }
    }
}
