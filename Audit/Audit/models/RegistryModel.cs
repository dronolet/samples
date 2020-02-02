using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Audit.models
{
    public class RegistryModel
    {
        public int Id { get; set; }

        public DateTime DBegin { get; set; }

        public DateTime DEnd { get; set; }

        public int  BuildObject { get; set; }

        public string Location { get; set; }

        public string WorkType { get; set; }


        public string Contractor { get; set; }

        public string Employee { get; set; }

        public int Result { get; set; }

        public int? head { get; set; }

        public string Remark { get; set; }

        public DateTime Repare { get; set; }

        public DateTime? RepareFakt { get; set; }

        public DateTime? BlockDate { get; set; }

        public int? UserId { get; set; }

        public int CanEdit { get; set; }
    }
}
