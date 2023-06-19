using System;
using Newtonsoft.Json;

namespace Parchmentum.Domain.BlogProviders;

public sealed class JsonFileParser : IFileParser
{
    public async Task<IList<Page>> ReadPages(Func<Task<IEnumerable<JsonFile>>> getFiles)
    {
        var files = await getFiles();

        var pages = files
            .Select(file =>
            {
                try
                {
                    return JsonConvert.DeserializeObject<Page>(file.Json) ?? throw new Exception($"{file.Name} is empty");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error reading page {file.Name}", ex);
                }
            })
            .ToList();

        return pages;
    }

    public async Task<IList<Post>> ReadPosts(Func<Task<IEnumerable<JsonFile>>> getFiles)
    {
        var files = await getFiles();

        var posts = files
            .Select(file =>
            {
                try
                {
                    return JsonConvert.DeserializeObject<Post>(file.Json) ?? throw new Exception($"{file.Name} is empty");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error reading post {file.Name}", ex);
                }
            })
            .ToList();

        return posts;
    }
}
