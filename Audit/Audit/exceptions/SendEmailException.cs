using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Audit.exceptions
{
    public class SendEmailException : Exception
    {
        public SendEmailException(string message) : base(message) {

        }
    }

  
}
