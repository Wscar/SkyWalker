using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Exceptions
{
    public class SkyWalkerException:Exception
    {
        public SkyWalkerException():base()
        {

        }
        public SkyWalkerException(string message):base(message)
        {

        }
        public SkyWalkerException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
