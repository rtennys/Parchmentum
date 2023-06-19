using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;

namespace Parchmentum.Domain.BlogProviders;

public sealed class AppDataBlogProvider : IBlogProvider
{
    public AppDataBlogProvider(IHostEnvironment hostEnvironment, IMemoryCache cache)
    {
        _hostEnvironment = hostEnvironment;

        IFileParser parser = new CachingFileParser(new JsonFileParser(), cache);
        _pages = new(() => Task.Run(() => parser.ReadPages(GetPages)).Result);
        _posts = new(() => Task.Run(() => parser.ReadPosts(GetPosts)).Result);
    }

    private readonly IHostEnvironment _hostEnvironment;

    private readonly Lazy<IList<Page>> _pages;
    private readonly Lazy<IList<Post>> _posts;

    public IQueryable<Page> Pages => _pages.Value.AsQueryable();
    public IQueryable<Post> Posts => _posts.Value.AsQueryable();

    private async Task<IEnumerable<JsonFile>> GetPages()
    {
        return await GetFiles("pages");
    }

    private async Task<IEnumerable<JsonFile>> GetPosts()
    {
        return await GetFiles("posts");
    }

    private async Task<IEnumerable<JsonFile>> GetFiles(string path)
    {
        var tasks = Directory
            .GetFiles(Path.Combine(_hostEnvironment.ContentRootPath, "App_Data", path), "*.json")
            .Select(async x => new JsonFile(x, await File.ReadAllTextAsync(x)));

        return await Task.WhenAll(tasks);
    }
}
