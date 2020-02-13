using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Vendr.Checkout.Web.Controllers
{
    public class VendrCheckoutStepPageController : RenderMvcController
    {
        public override ActionResult Index(ContentModel model)
        {
            // If the page has a template, use it
            if (model.Content.TemplateId.HasValue)
                return base.Index(model);

            // Get the current step from the page and render the appropriate view
            return View($"~/App_Plugins/VendrCheckout/Common/Views/VendrCheckout{model.Content.Value<string>("vendrStepType")}Page.cshtml", model);
        }
    }
}