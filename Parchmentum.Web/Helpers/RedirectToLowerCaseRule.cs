using System;
using Microsoft.AspNetCore.Rewrite;

namespace Parchmentum.Web.Helpers;

public sealed class RedirectToLowerCaseRule : IRule
{
    public void ApplyRule(RewriteContext context)
    {
        var request = context.HttpContext.Request;
        var path = request.Path;
        var pathBase = request.PathBase;
        if (path.HasValue && path.Value.Any(char.IsUpper) || pathBase.HasValue && pathBase.Value.Any(char.IsUpper))
        {
            context.Result = RuleResult.EndResponse;
            context.HttpContext.Response.Redirect((pathBase.Value + path.Value).ToLowerInvariant());
        }
    }
}
