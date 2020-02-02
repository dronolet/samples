using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apitest.Exceptions
{
    public class NotFindException:Exception
    {
        public NotFindException(string message) : base(message)
        {

        }
    }
}
