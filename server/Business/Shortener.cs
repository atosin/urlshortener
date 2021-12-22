using server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Business
{
    public class Shortener : IShortener
    {
        private readonly IShortenerData _shortenerData;

        public Shortener(IShortenerData shortenerData)
        {
            _shortenerData = shortenerData;
        }
        public string GetActualUrl(string hash)
        {
            var url = _shortenerData.GetActualUrlByHash(hash);

            if (url == default)
            {
                return url;
            }

            _shortenerData.UpdateLastAccessedDateForHash(hash);

            return url;
        } 

        public string GetHashFromUrl(string url)
        {
            if (!IsValidUrl(url))
            {
                return default;
            }

            //If hash,url combinations already exist, provide the hash and update the 
            //last accessed date
            if (_shortenerData.GetHashByUrl(url, out var hash))
            {
                _shortenerData.UpdateLastAccessedDateForHash(hash);

                return hash;
            }

            hash = GenerateHash();

            _shortenerData.SaveShortenedUrl(hash, url);

            return hash;
        }

        private string GenerateHash()
        {
            var hash = Guid.NewGuid().ToString("N").Substring(0, 7);

            var tries = 0;

            //In case of collision, try three times to generate hash
            while (_shortenerData.DoesHashExist(hash) && tries < 3)
            {
                hash = Guid.NewGuid().ToString("N").Substring(0, 7);
                tries++;
            }


            return hash;
        }

        private bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
