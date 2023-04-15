namespace TinyUrlAPI.Services;
public class BaseUrlService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _baseUrl;

    public BaseUrlService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/";
    }

    public string GetBaseUrl()
    {
        return _baseUrl;
    }
}
