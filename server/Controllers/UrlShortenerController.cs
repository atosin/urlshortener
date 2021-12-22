using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using server.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UrlShortenerController : ControllerBase
    {
        private readonly IShortener _shortener;
        public UrlShortenerController(IShortener shortener)
        {
            _shortener = shortener;
        }
        // GET api/<UrlShortenerController>/5
        [HttpGet("{slug}")]
        public string GetActualUrl(string slug)
        {
            NotNullOrWhiteSpace(slug, nameof(slug));
            return _shortener.GetActualUrl(slug);        
        }

        [HttpPost()]
        public string GetSlugForUrl([FromBody] string url)
        {
            NotNullOrWhiteSpace(url, nameof(url));
            return _shortener.GetSlugForUrl(url);
        }

        private static void NotNullOrWhiteSpace(string argument, string argumentName)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new ArgumentException($"'{argumentName}' cannot be null or empty");
            }
        }

    }
}
