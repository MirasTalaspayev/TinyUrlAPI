using Microsoft.Extensions.Caching.StackExchangeRedis;
using TinyUrlAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "SampleInstance";
});

builder.Services.AddSingleton<IUrlShortener, Base62ShortUrl>();
builder.Services.AddSingleton<TinyUrlService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Map("/{shortUrl}", (context) => {
    var tinyUrlService = app.Services.GetService<TinyUrlService>();
    var shortUrl = context.Request.RouteValues["shortUrl"]?.ToString();
    var fullUrl = tinyUrlService.GetFullUrl(shortUrl);

    if (fullUrl == null)
    {
        context.Response.Redirect("/");
    }
    else
    {
        context.Response.Redirect(fullUrl);
    }
    return Task.CompletedTask;
});

app.Run();
