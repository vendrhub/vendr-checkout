using Vendr.Checkout.Configuration;
using Microsoft.Extensions.Options;

namespace Vendr.Checkout
{
    public class PathHelper
    {
        private readonly string _rootViewPath;

        public PathHelper(IOptions<VendrCheckoutSettings> settings)
        {
            _rootViewPath = settings.Value.RootViewPath;
        }

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
