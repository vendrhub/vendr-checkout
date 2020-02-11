using Umbraco.Web;
using Umbraco.Core.Models.PublishedContent;
using Vendr.Core.Models;
using Umbraco.Core;

using Constants = Vendr.Core.Constants;

namespace Vendr.Checkout.Web
{
    public static class PublishedContentExtensions
    {
        public static string GetMetaTitle(this IPublishedContent content)
        {
            var storeName = content.Value<string>("vendrStoreName", fallback:Fallback.ToAncestors);
            var pageTitle = content.Value<string>("pageTitle");

            if (!pageTitle.IsNullOrWhiteSpace())
                return $"{pageTitle} | {storeName}";

            return $"{content.Name} | {storeName}";
        }

        public static StoreReadOnly GetStore(this IPublishedContent content)
        {
            return content.Value<StoreReadOnly>(Constants.Properties.StorePropertyAlias, fallback:Fallback.ToAncestors);
        }
    }
}
