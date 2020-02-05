using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apitest.Exceptions
{
    public class BadParametersException : Exception
    {
        public BadParametersException(string message) : base(message) {

        }
    }
}
