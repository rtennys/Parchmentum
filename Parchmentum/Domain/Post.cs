using System;
using Newtonsoft.Json;

namespace Parchmentum.Domain;

public sealed class Post
{
    public Guid Id { get; set; }
    public DateTime PubDate { get; init; }
    public DateTime LastModified { get; init; }
    public bool IsDeleted { get; init; }
    public bool IsPublished { get; init; }
    public bool IsCommentsEnabled { get; init; }
    public string Slug { get; init; } = null!;
    public string Title { get; init; } = null!;
    public string Content { get; set; } = null!;
    public IList<string>? Tags { get; init; }
    public IList<Comment>? Comments { get; init; }

    [JsonIgnore]
    public string TagString => Tags == null ? "" : string.Join(", ", Tags);

    public IEnumerable<Comment> NonTrackbackComments()
    {
        return (Comments ?? Array.Empty<Comment>()).Where(x => !string.Equals(x.Email, "trackback", StringComparison.OrdinalIgnoreCase));
    }

    public int NonTrackbackCommentCount()
    {
        return NonTrackbackComments().Count();
    }
}
