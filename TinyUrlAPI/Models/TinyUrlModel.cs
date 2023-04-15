using MongoDB.Bson;

namespace TinyUrlAPI.Models;
public class TinyUrlModel
{
    public ObjectId Id { get; set; }
    public string FullUrl { get; set; }
    public string ShortUrl { get; set; }
}

