using System;
using Microsoft.AspNetCore.Mvc;
using Parchmentum.Domain;

namespace Parchmentum.Web.Controllers;

public sealed class ArchiveController : Controller
{
    public ArchiveController(IBlogProvider blogProvider)
    {
        _blogProvider = blogProvider;
    }

    private readonly IBlogProvider _blogProvider;

    public IActionResult Index()
    {
        return View(new ArchiveIndexModel
        {
            Posts = _blogProvider.Posts
                .Where(x => !x.IsDeleted)
                .Where(x => x.IsPublished)
                .OrderByDescending(x => x.PubDate)
                .ToList()
        });
    }
}

public sealed class ArchiveIndexModel
{
    public List<Post> Posts { get; init; } = null!;
}
