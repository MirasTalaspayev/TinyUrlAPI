using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;
using MongoDB.Driver;
using TinyUrlAPI.Models;

namespace TinyUrlAPI.Services;
public class TinyUrlService
{
    private readonly IUrlShortener _urlShortener;
    private readonly IMongoClient _client;
    private readonly IMongoDatabase _database;
    private readonly IConfiguration _configuration;
    private readonly IDistributedCache _cache;

    public TinyUrlService(IUrlShortener urlShortener, IConfiguration configuration, IDistributedCache cache)
    {
        _urlShortener = urlShortener;
        _configuration = configuration;
        _client = new MongoClient(_configuration.GetConnectionString("mongoDb"));
        _database = _client.GetDatabase("TinyUrlDb");
        _cache = cache;
    }

    public TinyUrlModel GetShortUrlScheme(string url)
    {
        var collection = _database.GetCollection<TinyUrlModel>("TinyUrl");
        var filter = Builders<TinyUrlModel>.Filter.Eq("FullUrl", url);
        
        var tinyUrl = collection.Find(filter).FirstOrDefault();

        if (tinyUrl != null)
        {
            return tinyUrl;
        }

        tinyUrl = _urlShortener.ShortenUrl(url);
        collection.InsertOne(tinyUrl);

        return tinyUrl;
    }

    public string GetFullUrl(string shortUrl)
    {
        var collection = _database.GetCollection<TinyUrlModel>("TinyUrl");
        var filter = Builders<TinyUrlModel>.Filter.Eq("ShortUrl", shortUrl);

        var tinyUrl = collection.Find(filter).FirstOrDefault();

        if (tinyUrl != null)
        {
            return tinyUrl.FullUrl;
        }

        return string.Empty;
    }
}