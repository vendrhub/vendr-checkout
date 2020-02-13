using System;

namespace Vendr.Checkout
{
    public static class PathHelper
    {
        public const string StoresPath = "/App_Plugins/VendrCheckout";
        public const string VirtualStoresPath = "~" + StoresPath;
        private const string VirtualStorePathToken = "~" + StoresPath + "/{0}/";
        private const string VirtualStoreViewPathToken = "~" + StoresPath + "/{0}/Views/{1}.cshtml";
        private const string VirtualStorePartialViewPathToken = "~" + StoresPath + "/{0}/Views/Partials/{1}.cshtml";

        public static string GetStorePath(string storeAlias)
        {
            return string.Format(VirtualStorePathToken, storeAlias);
        }

        public static string GetStoreViewPath(string storeAlias, string viewName)
        {
            return string.Format(VirtualStoreViewPathToken, storeAlias, viewName);
        }

        public static string GetStorePartialViewPath(string storeAlias, string viewName)
        {
            return string.Format(VirtualStorePartialViewPathToken, storeAlias, viewName);
        }

        public static string GetDomain(Uri requestUrl)
        {
            return requestUrl.Scheme + Uri.SchemeDelimiter + requestUrl.Authority;
        }
    }
}