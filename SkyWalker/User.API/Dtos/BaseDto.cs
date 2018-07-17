using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Dtos
{
    public class BaseDto
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public EntityStatus Status { get; set; }
        public string Message { get; set; }
        public string DevelopmentMsg { get; set; }
    }
}
