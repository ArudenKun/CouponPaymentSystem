﻿using System;
using System.IO;

namespace Htmx;

public static class HtmxSnippets
{
    public static string AntiforgeryJavaScript => GetString(nameof(AntiforgeryJavaScript));

    public static string AntiforgeryJavaScriptMinified =>
        GetString(nameof(AntiforgeryJavaScriptMinified));

    public static string GetAntiforgeryJavaScript(bool minified) =>
        minified ? AntiforgeryJavaScriptMinified : AntiforgeryJavaScript;

    private static string GetString(string name)
    {
        var assembly = typeof(HtmxSnippets).Assembly;
        using var resource = assembly.GetManifestResourceStream(name);

        if (resource == null)
            throw new ArgumentException($"Resource \"{name}\" was not found.", nameof(name));

        using var reader = new StreamReader(resource);
        return reader.ReadToEnd();
    }
}
