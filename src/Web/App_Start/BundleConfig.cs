using System.Web.Optimization;
using BundleTransformer.Core.Bundles;
using BundleTransformer.Core.Orderers;

namespace Web;

public class BundleConfig
{
    // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
    public static void RegisterBundles(BundleCollection bundles)
    {
        bundles.Add(
            new CustomStyleBundle("~/bundles/css/base")
                .NullOrderer()
                .Include("~/Lib/bootstrap/css/bootstrap.min.css")
                .Include("~/Lib/bootstrap-icons/font/bootstrap-icons.min.css")
                .Include("~/Content/css/style.css")
        );

        bundles.Add(
            new CustomStyleBundle("~/bundles/css/datatables")
                .NullOrderer()
                .Include("~/Lib/datatables/datatables.min.css")
        );

        bundles.Add(
            new CustomScriptBundle("~/bundles/js/base/preload")
                .NullOrderer()
                .Include("~/Lib/jquery/jquery.min.js")
        );

        bundles.Add(
            new CustomScriptBundle("~/bundles/js/base")
                .NullOrderer()
                .Include("~/Lib/jquery/jquery.unobtrusive-ajax.min.js")
                .Include("~/Lib/bootstrap/js/bootstrap.bundle.min.js")
                .Include("~/Lib/sweetalert2/sweetalert2.all.min.js")
                .Include("~/Scripts/script.js")
        );

        bundles.Add(
            new CustomScriptBundle("~/bundles/js/jqueryval")
                .NullOrderer()
                .IncludeDirectory("~/Lib/jquery-validate", "*.js")
        );

        bundles.Add(
            new CustomScriptBundle("~/bundles/js/datatables")
                .NullOrderer()
                .Include("~/Lib/datatables/datatables.min.js")
        );

        BundleTable.EnableOptimizations = !IsDebug;
        // BundleTable.EnableOptimizations = true;
    }

    private static bool IsDebug
#if DEBUG
        => true;
#else
        => false;
#endif
}

public static class BundleExtensions
{
    private static readonly IBundleOrderer NullBundleOrderer = new NullOrderer();

    public static Bundle NullOrderer(this Bundle bundle) => bundle.WithOrderer(NullBundleOrderer);

    public static Bundle WithOrderer(this Bundle bundle, IBundleOrderer orderer)
    {
        bundle.Orderer = orderer;
        return bundle;
    }
}
