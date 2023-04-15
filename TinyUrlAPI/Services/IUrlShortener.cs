using TinyUrlAPI.Models;

namespace TinyUrlAPI.Services;
public interface IUrlShortener
{
    public TinyUrlModel ShortenUrl(string url);
}

