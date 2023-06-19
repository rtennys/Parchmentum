using System;
using Microsoft.Extensions.Caching.Memory;

namespace Parchmentum.Domain.BlogProviders;

public sealed class CachingFileParser : IFileParser
{
    public CachingFileParser(IFileParser fileParser, IMemoryCache cache)
    {
        _fileParser = fileParser;
        _cache = cache;
    }

    private readonly IFileParser _fileParser;
    private readonly IMemoryCache _cache;

    public async Task<IList<Page>> ReadPages(Func<Task<IEnumerable<JsonFile>>> getFiles)
    {
        return await _cache.GetOrCreate("pages", async cacheEntry =>
        {
            cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            var pages = await _fileParser.ReadPages(getFiles);
            return pages;
        })!;
    }

    public async Task<IList<Post>> ReadPosts(Func<Task<IEnumerable<JsonFile>>> getFiles)
    {
        return await _cache.GetOrCreate("posts", async cacheEntry =>
        {
            cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            var posts = await _fileParser.ReadPosts(getFiles);
            return posts;
        })!;
    }
}
