using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Vendr.Checkout.Web
{
    public static class HtmlHelperExtensions
    {

        private static PathHelper GetPathHelper(IHtmlHelper helper)
            => (PathHelper)helper.ViewContext.HttpContext.RequestServices.GetService(typeof(PathHelper));

        public static IHtmlContent VendrCheckoutPartial(this IHtmlHelper helper, string partialName)
        {
            var pathHelper = GetPathHelper(helper);

            return helper.Partial(pathHelper.GetVendrCheckoutPartialViewPath(partialName));
        }

        public static IHtmlContent VendrCheckoutPartial(this IHtmlHelper helper, string partialName, object model)
        {
            var pathHelper = GetPathHelper(helper);

            return helper.Partial(pathHelper.GetVendrCheckoutPartialViewPath(partialName), model);
        }
    }
}
