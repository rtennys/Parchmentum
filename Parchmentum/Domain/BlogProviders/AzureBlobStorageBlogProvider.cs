using System;
using System.Text;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Parchmentum.Domain.BlogProviders;

public sealed class AzureBlobStorageBlogProvider : IBlogProvider
{
    public AzureBlobStorageBlogProvider(IConfiguration configuration, IMemoryCache cache)
    {
        _configuration = configuration;
        IFileParser parser = new CachingFileParser(new JsonFileParser(), cache);
        _pages = new(() => Task.Run(() => parser.ReadPages(GetPages)).Result);
        _posts = new(() => Task.Run(() => parser.ReadPosts(GetPosts)).Result);
    }

    private readonly IConfiguration _configuration;

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

    private async Task<List<JsonFile>> GetFiles(string path)
    {
        var serviceClient = new BlobServiceClient(_configuration["Azure:BlobConnectionString"]);
        var containerClient = serviceClient.GetBlobContainerClient(_configuration["Azure:BlobContainerName"]);

        var files = new List<JsonFile>();

        await foreach (var blob in containerClient.GetBlobsAsync(prefix: path))
        {
            using var stream = new MemoryStream();
            var blobClient = containerClient.GetBlobClient(blob.Name);
            await blobClient.DownloadToAsync(stream);
            var json = Encoding.UTF8.GetString(stream.ToArray());

            files.Add(new JsonFile(blob.Name, json));
        }

        return files;
    }
}
