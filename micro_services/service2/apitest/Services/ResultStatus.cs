using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using apitest.Interfaces;
using apitest.Models;
using apitest.Modules;
using apitest.Objects;
using apitest.Exceptions;
using apitest.Classes;

namespace apitest.Services
{
   
    public  class ResultStatus: IResultStatus
    {
        public string status { get; set; }
        public object result { get; set; }
        public string message { get; set; }
    }
}
