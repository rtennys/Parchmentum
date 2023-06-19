using System;

namespace Parchmentum.Domain;

public sealed class Page
{
    public Guid Id { get; set; }
    public DateTime DateCreated { get; init; }
    public DateTime DateModified { get; init; }
    public bool IsDeleted { get; init; }
    public bool IsPublished { get; init; }
    public bool IsFrontPage { get; init; }
    public string Slug { get; init; } = null!;
    public string Title { get; init; } = null!;
    public string Content { get; set; } = null!;
}
