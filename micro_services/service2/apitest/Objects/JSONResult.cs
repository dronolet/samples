using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apitest.Objects
{
   
    public class JSONResult
    {
        public string status { get; set; }

        public Company[] result { get; set; }
    }
}
