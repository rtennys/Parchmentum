using System;
using Microsoft.AspNetCore.Mvc;
using Parchmentum.Domain;

namespace Parchmentum.Web.Controllers;

public sealed class BlogController : Controller
{
    public BlogController(IBlogProvider blogProvider)
    {
        _blogProvider = blogProvider;
    }

    private readonly IBlogProvider _blogProvider;

    public IActionResult Index([FromQuery] int? page)
    {
        if (page < 1)
            return RedirectToAction(nameof(Index));

        var posts = _blogProvider.Posts
            .Where(x => !x.IsDeleted)
            .Where(x => x.IsPublished)
            .OrderByDescending(x => x.PubDate)
            .ToPagedList(page ?? 1, 5);

        if (posts.PageCount < page)
            return RedirectToAction(nameof(Index), new { page = posts.PageCount });

        return View(new BlogIndexModel
        {
            Posts = posts
        });
    }
}

public sealed class BlogIndexModel
{
    public PagedList<Post> Posts { get; init; } = default!;
}
