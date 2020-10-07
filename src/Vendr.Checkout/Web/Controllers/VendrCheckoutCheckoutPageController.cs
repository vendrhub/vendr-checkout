using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Vendr.Checkout.Web.Controllers
{
    public class VendrCheckoutCheckoutPageController : RenderMvcController
    {
        public override ActionResult Index(ContentModel model)
        {
            // If the page has a template, use it
            if (model.Content.TemplateId.HasValue && model.Content.TemplateId.Value > 0)
                return base.Index(model);

            // No template so redirect to the first child if one exists
            if (model.Content.Children != null)
            {
                var firstChild = model.Content.Children.FirstOrDefault();
                if (firstChild != null)
                {
                    return RedirectPermanent(firstChild.Url);
                }
            }

            // Still nothing so 404
            return new HttpNotFoundResult();
        }
    }
}