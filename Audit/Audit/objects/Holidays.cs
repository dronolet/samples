using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Audit.objects
{
    [Table("Holidays")]
    public class Holiday
    {
        [Key]
        public DateTime Data { get; set; }
        public int? Hours { get; set; }
        public Int16 DayOffset { get; set; }
    }
}
