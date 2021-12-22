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
        public string GetActualUrl(string slug)
        {
            var url = _shortenerData.GetActualUrlBySlug(slug);

            if (url == default)
            {
                return url;
            }

            _shortenerData.UpdateLastAccessedDateForSlug(slug);

            return url;
        } 

        public string GetSlugForUrl(string url)
        {
            if (!IsValidUrl(url))
            {
                return default;
            }

            //If slug,url combinations already exist, provide the slug and update the 
            //last accessed date
            if (_shortenerData.GetSlugByUrl(url, out var slug))
            {
                _shortenerData.UpdateLastAccessedDateForSlug(slug);

                return slug;
            }

            slug = GenerateSlug();

            _shortenerData.SaveShortenedUrl(slug, url);

            return slug;
        }

        private string GenerateSlug()
        {
            var slug = Guid.NewGuid().ToString("N").Substring(0, 7);

            var tries = 0;

            //In case of collision, try three times to generate slug
            while (_shortenerData.DoesSlugExist(slug) && tries < 3)
            {
                slug = Guid.NewGuid().ToString("N").Substring(0, 7);
                tries++;
            }


            return slug;
        }

        private bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
