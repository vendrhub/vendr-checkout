#if NETFRAMEWORK
using System.Web.Mvc.Html;
using Umbraco.Core.Composing;
using IHtmlHelper = System.Web.Mvc.HtmlHelper;
using IHtmlContent = System.Web.IHtmlString;
#else
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
#endif

namespace Vendr.Checkout.Web
{
    public static class HtmlHelperExtensions
    {

#if NETFRAMEWORK
        private static PathHelper GetPathHelper(IHtmlHelper helper)
            =>  (PathHelper)Current.Factory.GetInstance(typeof(PathHelper));
#else
        private static PathHelper GetPathHelper(IHtmlHelper helper)
            => (PathHelper)helper.ViewContext.HttpContext.RequestServices.GetService(typeof(PathHelper));
#endif

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
