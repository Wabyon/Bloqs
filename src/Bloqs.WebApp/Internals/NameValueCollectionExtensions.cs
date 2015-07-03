using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Bloqs.Internals
{
    internal static class NameValueCollectionExtensions
    {
        public static IDictionary<string, string> ToDictionary(this NameValueCollection nameValueCollection)
        {
            return nameValueCollection.AllKeys.ToDictionary(key => key, key => nameValueCollection[key]);
        }
    }
}