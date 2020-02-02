using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Audit.objects
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public DateTime DBegin { get; set; }

        public DateTime DEnd { get; set; }

        public int BuildObject { get; set; }

        public string Location { get; set; }

        public string Worktype { get; set; }

        public string Contractor { get; set; }

        public int Result { get; set; }

        public int? Head { get; set; }

        public string Remark { get; set; }

        public DateTime Repare { get; set; }

        public DateTime? RepareFakt { get; set; }

        public DateTime? BlockDate { get; set; }

        public int UserId { get; set; }

        public DateTime? dedited { get; set; }
    }
}
