using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apitest.Exceptions
{
    public class BadLogicException : Exception
    {
        public BadLogicException(string message) : base(message)
        {

        }
    }
}
