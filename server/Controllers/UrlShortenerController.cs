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
        [HttpGet("{hash}")]
        public string GetActualUrl(string hash)
        {
            return _shortener.GetActualUrl(hash);
         
        }

        [HttpPost()]
        public string GetHashFromUrl([FromBody] string url)
        {
            return _shortener.GetHashFromUrl(url);
        }

        //[HttpPost]
        //public string GetShortenedUrl([FromBody] string url)
        //{
        //    var shortenedUrl = "localhost.com/shortened";
        //    return shortenedUrl;
        //}

    }
}
