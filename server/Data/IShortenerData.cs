using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Data
{
    public interface IShortenerData
    { 
        string GetActualUrlBySlug(string slug);
        bool GetSlugByUrl(string url, out string slug);
        string SaveShortenedUrl(string slug, string url);
        void UpdateLastAccessedDateForSlug(string slug);
        bool DoesSlugExist(string slug);
    }
}
