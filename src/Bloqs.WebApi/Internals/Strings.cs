using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bloqs.Internals
{
    internal static class Strings
    {
        public static string EmptyToNull(this string s)
        {
            return string.IsNullOrWhiteSpace(s) ? null : s;
        }
    }
}