using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Dtos
{
    public class AccountResult
    {
        public string Status { get; set; }
        public UserInfo User { get; set; }
        public string Message { get; set; }
    }
}
