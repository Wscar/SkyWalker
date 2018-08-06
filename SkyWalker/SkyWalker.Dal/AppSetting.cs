using System;
using System.Collections.Generic;
using System.Text;

namespace SkyWalker.Dal
{
  public  class AppSetting
    {
        public string MongonContactConnectionString { get; set; }
        public string MongoDbDatabaseSkyWalker { get; set; }
        public string MySqlConnectionString { get; set; }
    }
}
