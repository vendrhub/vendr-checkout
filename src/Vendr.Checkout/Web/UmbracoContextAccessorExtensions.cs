#if NETFRAMEWORK

using Umbraco.Web;

namespace Vendr.Checkout.Web
{
    public static class UmbracoContextAccessorExtensions
    {
        // Extension method to standardize the API between v8 and v9
        public static UmbracoContext GetRequiredUmbracoContext(this IUmbracoContextAccessor umbracoContextAccessor)
            => umbracoContextAccessor.UmbracoContext;
    }
}

#endif