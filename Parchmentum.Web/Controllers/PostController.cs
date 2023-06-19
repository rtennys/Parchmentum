using System;
using Microsoft.AspNetCore.Mvc;
using Parchmentum.Domain;

namespace Parchmentum.Web.Controllers;

[Route("Post")]
public sealed class PostController : Controller
{
    public PostController(IBlogProvider blogProvider)
    {
        _blogProvider = blogProvider;
    }

    private readonly IBlogProvider _blogProvider;

    [Route("{slug}")]
    public IActionResult Index([FromRoute] string slug)
    {
        return View(new PostIndexModel
        {
            Post = _blogProvider.Posts
                .Where(x => !x.IsDeleted)
                .Where(x => x.IsPublished)
                .Where(x => string.Equals(x.Slug, slug, StringComparison.OrdinalIgnoreCase))
                .SingleOrDefault()
        });
    }
}

public sealed class PostIndexModel
{
    public Post? Post { get; init; }
}
