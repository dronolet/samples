using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Audit.objects
{
    [Table("HeadManagers")]
    public class HeadManager
    {
        [Key]
        public int id { get; set; }

        [Column("name")]
        public string name { get; set; }

        public string Email { get; set; }

        public int isAdmin { get; set; }
    }
}
