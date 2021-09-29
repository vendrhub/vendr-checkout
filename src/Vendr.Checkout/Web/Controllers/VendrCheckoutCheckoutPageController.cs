using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Models;

namespace Vendr.Checkout.Web.Controllers
{
    public class VendrCheckoutCheckoutPageController : VendrCheckoutBaseController
    {
        public override ActionResult Index(ContentModel model)
        {
            // Check the cart is valid before continuing 
            if (!IsValidCart(out var redirectUrl))
                return Redirect(redirectUrl);

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