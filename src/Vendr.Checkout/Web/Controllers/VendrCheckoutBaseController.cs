using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Vendr.Core.Api;
using Vendr.Core.Models;

namespace Vendr.Checkout.Web.Controllers
{
    public abstract class VendrCheckoutBaseController : RenderMvcController
    {
        protected bool IsValidCart(out string redirectUrl)
        { 
            var store = CurrentPage.Value<StoreReadOnly>(Core.Constants.Properties.StorePropertyAlias, fallback: Fallback.ToAncestors);
            var order = !IsConfirmationPageType(CurrentPage)
                ? VendrApi.Instance.GetCurrentOrder(store.Id)
                : VendrApi.Instance.GetCurrentFinalizedOrder(store.Id);

            if (order == null || order.OrderLines.Count == 0)
            {
                var backPage = CurrentPage.Value<IPublishedContent>("vendrCheckoutBackPage", fallback: Fallback.ToAncestors);
                redirectUrl = backPage != null ? backPage.Url : "/";
                return false;
            }

            redirectUrl = null;
            return true;
        }

        private bool IsConfirmationPageType(IPublishedContent node)
        {
            if (node == null || node.ContentType == null || !node.HasProperty("vendrStepType"))
                return false;

            return node.ContentType.Alias == VendrCheckoutConstants.ContentTypes.Aliases.CheckoutStepPage && node.Value<string>("vendrStepType") == "Confirmation";
        }
    }
}