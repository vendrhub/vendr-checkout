using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace Vendr.Checkout.Web.Controllers
{
    public class VendrCheckoutCheckoutStepPageController : VendrCheckoutBaseController
    {
        public override ActionResult Index(ContentModel model)
        {
            // Check the cart is valid before continuing 
            if (!IsValidCart(out var redirectUrl))
                return Redirect(redirectUrl);

            // If the page has a template, use it
            if (model.Content.TemplateId.HasValue && model.Content.TemplateId.Value > 0)
                return base.Index(model);

            // Get the current step from the page and render the appropriate view
            return View(PathHelper.GetVendrCheckoutViewPath($"VendrCheckout{model.Content.Value<string>("vendrStepType")}Page"), model);
        }
    }
}