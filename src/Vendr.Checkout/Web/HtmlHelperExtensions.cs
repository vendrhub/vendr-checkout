using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Vendr.Checkout.Web
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlString VendrCheckoutPartial(this HtmlHelper helper, string partialName)
        {
            return helper.Partial(PathHelper.GetVendrCheckoutPartialViewPath(partialName));
        }

        public static IHtmlString VendrCheckoutPartial(this HtmlHelper helper, string partialName, object model)
        {
            return helper.Partial(PathHelper.GetVendrCheckoutPartialViewPath(partialName), model);
        }
    }
}
