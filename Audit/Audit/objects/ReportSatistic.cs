using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Audit.objects
{
    public class ReportSatistic
    {
        [Key]
        public int id { get; set; }

        public string name { get; set; }


        public int outputs { get; set; }

        public int noremarks { get; set; }

        public int remarks { get; set; }

        public int notifs { get; set; }


        public int wremarks { get; set; }

        public int wnotifs { get; set; }


        public int oremarks { get; set; }

        public int onotifs { get; set; }
        
    }
}
