using Vendr.Core.Models;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Vendr.Checkout.Web
{
    public static class PublishedContentExtensions
    {
        public static StoreReadOnly GetStore(this IPublishedContent content)
        {
            return content.Value<StoreReadOnly>(Umbraco.Constants.Properties.StorePropertyAlias, fallback:Fallback.ToAncestors);
        }

        public static IPublishedContent GetCheckoutPage(this IPublishedContent content)
        {
            return content.AncestorOrSelf(VendrCheckoutConstants.ContentTypes.Aliases.CheckoutPage);
        }

        public static IPublishedContent GetCheckoutBackPage(this IPublishedContent content)
        {
            return GetCheckoutPage(content).Value<IPublishedContent>("vendrCheckoutBackPage");
        }

        public static string GetThemeColor(this IPublishedContent content)
        {
            var themeColor = GetCheckoutPage(content).Value("vendrThemeColor", defaultValue: "000000");

            if (VendrCheckoutConstants.ColorMap.ContainsKey(themeColor))
                return VendrCheckoutConstants.ColorMap[themeColor];

            if (VendrCheckoutConstants.ColorMapV1.ContainsKey(themeColor))
                return VendrCheckoutConstants.ColorMapV1[themeColor];

            return VendrCheckoutConstants.ColorMap.First().Value;
        }

        public static IPublishedContent GetPreviousPage(this IPublishedContent content)
        {
            return content.Parent.Children.TakeWhile(x => !x.Id.Equals(content.Id)).LastOrDefault();
        }

        public static IPublishedContent GetPreviousStepPage(this IPublishedContent content)
        {
            var prevPage = GetPreviousPage(content);
            if (prevPage == null)
                return null;

            var stepType = prevPage.Value<string>("vendrStepType");
            if (stepType == null || stepType != "ShippingMethod")
                return prevPage;

            var checkoutPage = GetCheckoutPage(content);
            if (checkoutPage.Value<bool>("vendrCollectShippingInfo"))
                return prevPage;

            return GetPreviousStepPage(prevPage);
        }

        public static IPublishedContent GetNextPage(this IPublishedContent content)
        {
            return content.Parent.Children.SkipWhile(x => !x.Id.Equals(content.Id)).Skip(1).FirstOrDefault();
        }
        public static IPublishedContent GetNextStepPage(this IPublishedContent content)
        {
            var nextPage = GetNextPage(content);
            if (nextPage == null)
                return null;

            var stepType = nextPage.Value<string>("vendrStepType");
            if (stepType == null || stepType != "ShippingMethod")
                return nextPage;

            var checkoutPage = GetCheckoutPage(content);
            if (checkoutPage.Value<bool>("vendrCollectShippingInfo"))
                return nextPage;

            return GetNextStepPage(nextPage);
        }
    }
}
