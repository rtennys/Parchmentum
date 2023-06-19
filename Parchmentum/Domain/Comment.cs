using System;

namespace Parchmentum.Domain;

public sealed class Comment
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Author { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Website { get; set; }
    public string Content { get; set; } = null!;
}
