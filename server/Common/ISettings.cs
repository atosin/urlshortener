using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Common
{
    public interface ISettings
    {
        public string DbServer { get; }
        public string DbName { get; }

    }
}
