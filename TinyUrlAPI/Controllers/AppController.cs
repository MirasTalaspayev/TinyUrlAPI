using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using TinyUrlAPI.Models;
using TinyUrlAPI.Services;

namespace TinyUrlAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppController : ControllerBase
    {
        private TinyUrlService _urlService;
        public AppController(TinyUrlService urlService)
        {
            _urlService = urlService;
        }
        [HttpPost]
        public string PostUrl([FromBody] UrlModel url)
        {
            return _urlService.GetShortUrlScheme(url.Url);
        }
    }
}
