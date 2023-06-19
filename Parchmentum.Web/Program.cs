using Microsoft.AspNetCore.Rewrite;
using Parchmentum.Domain;
using Parchmentum.Domain.BlogProviders;
using Parchmentum.Web.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

builder.Services.AddControllersWithViews();

builder.Services.Configure<RouteOptions>(x => x.LowercaseUrls = true);

switch (builder.Configuration.GetValue<string>("BlogProvider"))
{
    case "AzureBlobStorageBlogProvider":
        builder.Services.AddScoped<IBlogProvider, AzureBlobStorageBlogProvider>();
        break;
    case "AppDataBlogProvider":
        builder.Services.AddScoped<IBlogProvider, AppDataBlogProvider>();
        break;
    default:
        throw new Exception("No or invalid blog provider configured");
}

builder.WebHost.UseStaticWebAssets();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRewriter(new RewriteOptions()
    .AddRedirectToWww()
    .Add(new RedirectToLowerCaseRule()));

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
