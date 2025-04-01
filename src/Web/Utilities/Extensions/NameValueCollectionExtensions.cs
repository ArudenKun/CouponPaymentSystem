using System.Collections.Specialized;

namespace Web.Utilities.Extensions;

public static class NameValueCollectionExtensions
{
    public static bool ContainsKey(this NameValueCollection collection, string key)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));
        return collection[key] != null;
    }
}
