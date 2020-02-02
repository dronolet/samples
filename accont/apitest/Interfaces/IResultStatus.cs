using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apitest.Models;

namespace apitest.Interfaces
{
   
    public interface IResultStatus
    {
        string status { get; set; }
        object result { get; set; }
        string message { get; set; }
    }
}
