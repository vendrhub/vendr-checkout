#if NETFRAMEWORK
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Umbraco.Core.Composing;
using IHtmlContent = System.Web.IHtmlString;
#else
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
#endif

namespace Vendr.Checkout.Web
{
    public static class HtmlHelperExtensions
    {

#if NETFRAMEWORK
        private static PathHelper GetPathHelper(HtmlHelper helper)
            =>  (PathHelper)Current.Factory.GetInstance(typeof(PathHelper));
#else
        private static PathHelper GetPathHelper(HtmlHelper helper)
            => (PathHelper)helper.ViewContext.HttpContext.RequestServices.GetService(typeof(PathHelper));
#endif

        public static IHtmlContent VendrCheckoutPartial(this HtmlHelper helper, string partialName)
        {
            var pathHelper = GetPathHelper(helper);

            return helper.Partial(pathHelper.GetVendrCheckoutPartialViewPath(partialName));
        }

        public static IHtmlContent VendrCheckoutPartial(this HtmlHelper helper, string partialName, object model)
        {
            var pathHelper = GetPathHelper(helper);

            return helper.Partial(pathHelper.GetVendrCheckoutPartialViewPath(partialName), model);
        }
    }
}
