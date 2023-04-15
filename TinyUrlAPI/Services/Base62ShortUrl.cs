using TinyUrlAPI.Models;

namespace TinyUrlAPI.Services;
public class Base62ShortUrl : IUrlShortener
{
    private static HashSet<int> counters = new HashSet<int>();
    private static readonly char[] map = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
    private static readonly Random random = new Random();

    public TinyUrlModel ShortenUrl(string url)
    {
        return new TinyUrlModel() { FullUrl = url, ShortUrl = IdToShortUrl() };
    }
    private string IdToShortUrl()
    {
        var n = random.Next();

        while (counters.Contains(n))
        {
            n = random.Next();
        }

        string shorturl = "";

        // Convert given integer id to a base 62 number 
        while (n > 0)
        {
            // use above map to store actual character 
            // in short url 
            shorturl += (map[n % 62]);
            n /= 62;
        }

        return Reverse(shorturl);
    }
    private string Reverse(string input)
    {
        char[] a = input.ToCharArray();
        int l, r = a.Length - 1;
        for (l = 0; l < r; l++, r--)
        {
            (a[r], a[l]) = (a[l], a[r]);
        }
        return string.Join("", a);
    }
}

