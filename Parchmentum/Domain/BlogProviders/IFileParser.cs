using System;

namespace Parchmentum.Domain.BlogProviders;

public sealed record JsonFile(string Name, string Json);

public interface IFileParser
{
    Task<IList<Page>> ReadPages(Func<Task<IEnumerable<JsonFile>>> getFiles);
    Task<IList<Post>> ReadPosts(Func<Task<IEnumerable<JsonFile>>> getFiles);
}
