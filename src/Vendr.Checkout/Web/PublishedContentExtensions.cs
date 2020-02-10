using Umbraco.Web;
using Umbraco.Core.Models.PublishedContent;
using Vendr.Core;
using Vendr.Core.Models;

namespace Vendr.Checkout.Web
{
    public static class PublishedContentExtensions
    {
        public static StoreReadOnly GetStore(this IPublishedContent content)
        {
            return content.Value<StoreReadOnly>(Constants.Properties.StorePropertyAlias, fallback:Fallback.ToAncestors);
        }
    }
}
