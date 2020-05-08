using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Vendr.Checkout.Web.Controllers
{
    public class VendrCheckoutCheckoutStepPageController : RenderMvcController
    {
        public override ActionResult Index(ContentModel model)
        {
            // If the page has a template, use it
            if (model.Content.TemplateId.HasValue && model.Content.TemplateId.Value > 0)
                return base.Index(model);

            // Get the current step from the page and render the appropriate view
            return View(PathHelper.GetVendrCheckoutViewPath($"VendrCheckout{model.Content.Value<string>("vendrStepType")}Page"), model);
        }
    }
}