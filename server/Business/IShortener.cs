using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Business
{
    public interface IShortener
    {
        string GetHashFromUrl(string url);
        string GetActualUrl(string hash);
    }
}
