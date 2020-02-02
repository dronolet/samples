using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Audit.objects
{
    [Table("HeadStroyUser")]
    public class HeadStroyUser
    {
        [Key]
        public int id  { get; set; }

        [Column("name")]
        public string name { get; set; }

        public string Email { get; set; }

        
    }
}
