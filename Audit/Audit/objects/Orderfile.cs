using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Audit.objects
{

            
    [Table("OrderFiles")]
    public class OrderFile
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }

        public string Filename { get; set; }

        public string RepFileName { get; set; }

        public int userid { get; set; }

    }
}
