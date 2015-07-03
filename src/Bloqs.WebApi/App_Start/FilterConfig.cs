using System.Web.Http.Filters;
using Bloqs.Filters;

namespace Bloqs
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(HttpFilterCollection filters)
        {
            filters.Add(new GlobalHandleErrorAttribute());
        }

    }
}