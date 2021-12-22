using Microsoft.Extensions.Configuration;
using server.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Controllers
{
    public class Settings: ISettings
    {
        private readonly IConfiguration _configuration;

        public Settings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string DbServer => GetValue("DbServer");

        public string DbName => GetValue("DbName");

        private string GetValue(string key)
        {
            return _configuration.GetSection(key).Value;
        }
    }
}
