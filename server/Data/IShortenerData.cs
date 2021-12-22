using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Data
{
    public interface IShortenerData
    { 
        string GetActualUrlByHash(string hash);
        bool GetHashByUrl(string url, out string hash);
        string SaveShortenedUrl(string hash, string url);
        void UpdateLastAccessedDateForHash(string hash);
        bool DoesHashExist(string hash);
    }
}
