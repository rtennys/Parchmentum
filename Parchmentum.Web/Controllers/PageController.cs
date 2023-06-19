using System;
using Microsoft.AspNetCore.Mvc;
using Parchmentum.Domain;

namespace Parchmentum.Web.Controllers;

[Route("Page")]
public class PageController : Controller
{
    public PageController(IBlogProvider blogProvider)
    {
        _blogProvider = blogProvider;
    }

    private readonly IBlogProvider _blogProvider;

    [Route("{slug}")]
    public IActionResult Index([FromRoute] string slug)
    {
        return View(new PageIndexModel
        {
            Page = _blogProvider.Pages
                .Where(x => !x.IsDeleted)
                .Where(x => x.IsPublished)
                .Where(x => string.Equals(x.Slug, slug, StringComparison.OrdinalIgnoreCase))
                .SingleOrDefault()
        });
    }
}

public sealed class PageIndexModel
{
    public Page? Page { get; init; }
}
