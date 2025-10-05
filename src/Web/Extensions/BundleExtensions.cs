using System.Web.Optimization;
using BundleTransformer.Core.Orderers;

namespace Web.Extensions;

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
