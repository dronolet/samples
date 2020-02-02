using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Audit.models
{
    public class GetOrdersModel
    {
        public DateTime? dfrom { get; set; }

        public DateTime? dto { get; set; }

        public int overdue { get; set; }
    }
}
