using System;
using System.Reflection;

namespace Parchmentum;

public static class AppVersion
{
    private static readonly Lazy<string> _version = new(GetVersion);

    public static string Version => _version.Value;

    private static string GetVersion()
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            var attr = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            return attr?.InformationalVersion ?? "AssemblyInformationalVersion not found";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}
