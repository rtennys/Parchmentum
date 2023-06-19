using System;

namespace Parchmentum.Domain;

public interface IBlogProvider
{
    IQueryable<Page> Pages { get; }
    IQueryable<Post> Posts { get; }
}
