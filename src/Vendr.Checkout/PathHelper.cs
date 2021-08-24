using Vendr.Checkout.Configuration;

#if NETFRAMEWORK

#else
using Microsoft.Extensions.Options;
#endif

namespace Vendr.Checkout
{
    public class PathHelper
    {
        private readonly string _rootViewPath;

#if NETFRAMEWORK
        public PathHelper(VendrCheckoutSettings settings)
        {
            _rootViewPath = settings.RootViewPath;
        }
#else
        public PathHelper(IOptions<VendrCheckoutSettings> settings)
        {
            _rootViewPath = settings.Value.RootViewPath;
        }
#endif

        public string GetVendrCheckoutViewPath(string viewName)
        {
            return string.Format("~" + _rootViewPath + "/{0}.cshtml", viewName);
        }

        public string GetVendrCheckoutPartialViewPath(string viewName)
        {
            return string.Format("~" + _rootViewPath + "/Partials/{0}.cshtml", viewName);
        }

        public string GetVendrCheckoutEmailViewPath(string viewName)
        {
            return string.Format("~" + _rootViewPath + "/Emails/{0}.cshtml", viewName);
        }
    }
}
