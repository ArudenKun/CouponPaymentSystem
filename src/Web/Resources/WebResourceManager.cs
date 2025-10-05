using System.Collections.Immutable;
using System.Web.WebPages;
using Abp.Collections.Extensions;
using Abp.Extensions;
using Abp.Timing;
using Cps.Core.Common;

namespace Web.Resources;

public class WebResourceManager
{
    private readonly List<string> _scriptUrls;

    public WebResourceManager()
    {
        _scriptUrls = [];
    }

    public void AddScript(string url, bool addMinifiedOnProd = true)
    {
        _scriptUrls.AddIfNotContains(NormalizeUrl(url, "js"));
    }

    public IReadOnlyList<string> GetScripts()
    {
        return _scriptUrls.ToImmutableList();
    }

    public HelperResult RenderScripts()
    {
        return new HelperResult(
            async void (writer) =>
            {
                foreach (var scriptUrl in _scriptUrls)
                {
                    await writer.WriteAsync(
                        $"<script src=\"{scriptUrl}?v=" + Clock.Now.Ticks + "\"></script>"
                    );
                }
            }
        );
    }

    private string NormalizeUrl(string url, string ext)
    {
        if (DebugHelper.IsDebug)
        {
            return url;
        }

        if (url.EndsWith(".min." + ext))
        {
            return url;
        }

        return url.Left(url.Length - ext.Length) + "min." + ext;
    }
}
