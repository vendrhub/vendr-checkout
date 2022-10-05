using Vendr.Core.Api;
using Vendr.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Extensions;

namespace Vendr.Checkout.Web.Controllers
{
    public abstract class VendrCheckoutBaseController : RenderController
    {
        public VendrCheckoutBaseController(ILogger<VendrCheckoutBaseController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        { }

        protected bool IsValidCart(out string redirectUrl)
        { 
            var store = CurrentPage.Value<StoreReadOnly>(Vendr.Umbraco.Constants.Properties.StorePropertyAlias, fallback: Fallback.ToAncestors);
            var order = !IsConfirmationPageType(CurrentPage)
                ? VendrApi.Instance.GetCurrentOrder(store.Id)
                : VendrApi.Instance.GetCurrentFinalizedOrder(store.Id);

            if (order == null || order.OrderLines.Count == 0)
            {
                var backPage = CurrentPage.Value<IPublishedContent>("vendrCheckoutBackPage", fallback: Fallback.ToAncestors);
                redirectUrl = backPage != null ? backPage.Url() : "/";
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