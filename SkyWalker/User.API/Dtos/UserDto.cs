using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SkyWalker.Dal.Entities;
namespace User.API.Dtos
{  
   public enum EntityStatus
    {
        Suceess,
        Fail
    }
    public class UserDto:BaseDto
    {
     
       public AppUser User { get; set; }
    }
}
