using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bakis.Models
{
    public class GroupRight
    {
        [Key]
        public int GroupRightId { get; set; }
        [ForeignKey("GroupId")]
        public int GroupId { get; set; }
        [ForeignKey("RightId")]
        public int RightId { get; set; }
    }
}
