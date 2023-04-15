using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;
using MongoDB.Driver;
using TinyUrlAPI.Models;
using ZstdSharp.Unsafe;

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

    public string GetShortUrlScheme(string fullUrl)
    {
        var shortUrl = _cache.GetString(fullUrl);
        if (shortUrl != null)
        {
            Console.WriteLine(shortUrl + " is taken from cache");
            return shortUrl;
        }

        var collection = _database.GetCollection<TinyUrlModel>("TinyUrl");
        var filter = Builders<TinyUrlModel>.Filter.Eq("FullUrl", fullUrl);
        
        var tinyUrl = collection.Find(filter).FirstOrDefault();

        if (tinyUrl != null)
        {
            return tinyUrl.ShortUrl;
        }

        tinyUrl = _urlShortener.ShortenUrl(fullUrl);
        collection.InsertOne(tinyUrl);

        _cache.SetString(fullUrl, tinyUrl.ShortUrl);
        return tinyUrl.ShortUrl;
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