using System;
using System.Security.Cryptography;
using System.Text;

namespace Parchmentum;

public static class GravatarHelper
{
    public static string GetGravatarLink(string email)
    {
        var bytes = Encoding.ASCII.GetBytes(email.Trim().ToLower());
        var hash = MD5.HashData(bytes);

        var sb = new StringBuilder();
        foreach (var b in hash)
            sb.Append(b.ToString("x2"));

        return $"https://www.gravatar.com/avatar/{sb}";
    }
}
