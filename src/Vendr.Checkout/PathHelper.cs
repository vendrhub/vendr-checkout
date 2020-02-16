using System;

namespace Vendr.Checkout
{
    public static class PathHelper
    {
        public const string RootPath = "/App_Plugins/VendrCheckout";
        public const string VirtualRootPath = "~" + RootPath;
        private const string VirtualViewPathToken = "~" + RootPath + "/Views/{0}.cshtml";
        private const string VirtualPartialViewPathToken = "~" + RootPath + "/Views/Partials/{0}.cshtml";
        private const string VirtualEmailViewPathToken = "~" + RootPath + "/Views/Emails/{0}.cshtml";

        public static string GetVendrCheckoutViewPath(string viewName)
        {
            return string.Format(VirtualViewPathToken, viewName);
        }

        public static string GetVendrCheckoutPartialViewPath(string viewName)
        {
            return string.Format(VirtualPartialViewPathToken, viewName);
        }
        public static string GetVendrCheckoutEmailViewPath(string viewName)
        {
            return string.Format(VirtualEmailViewPathToken, viewName);
        }

        public static string GetDomain(Uri requestUrl)
        {
            return requestUrl.Scheme + Uri.SchemeDelimiter + requestUrl.Authority;
        }
    }
}