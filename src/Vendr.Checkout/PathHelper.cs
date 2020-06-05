using System;
using System.Configuration;

namespace Vendr.Checkout
{
    public static class PathHelper
    {
        public static string RootPath
        {
            get
            {
                var rootPath = ConfigurationManager.AppSettings["VendrCheckout:RootPath"]?.ToString();
                return !string.IsNullOrWhiteSpace(rootPath) ? rootPath : "/App_Plugins/VendrCheckout";
            } 
        }
          
        private static readonly string VirtualViewPathToken = "~" + RootPath + "/Views/{0}.cshtml";
        private static readonly string VirtualPartialViewPathToken = "~" + RootPath + "/Views/Partials/{0}.cshtml";
        private static readonly string VirtualEmailViewPathToken = "~" + RootPath + "/Views/Emails/{0}.cshtml";

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
