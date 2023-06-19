using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Parchmentum.Domain;

namespace Parchmentum.Web.Controllers;

public sealed class HomeController : Controller
{
    public HomeController(IBlogProvider blogProvider)
    {
        _blogProvider = blogProvider;
    }

    private readonly IBlogProvider _blogProvider;

    public IActionResult Index()
    {
        Page homePage;
        try
        {
            homePage = _blogProvider.Pages
                .Where(x => !x.IsDeleted)
                .Where(x => x.IsPublished)
                .Where(x => x.IsFrontPage)
                .Single();
        }
        catch (Exception ex)
        {
            homePage = new Page
            {
                Content = $"<pre>{ex}</pre>"
            };
        }

        return View(new HomeIndexModel
        {
            HomePage = homePage
        });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

public sealed class HomeIndexModel
{
    public Page HomePage { get; set; } = null!;
}

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
