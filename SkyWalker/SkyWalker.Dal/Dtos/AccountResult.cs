using System;
using System.Collections.Generic;
using System.Text;
using SkyWalker.Dal.Entities;
namespace SkyWalker.Dal.Dtos
{
   public class AccountResult
    {
        public string Status { get; set; }
        public AppUser User { get; set; }
        public string Message { get; set; }
    }
}
