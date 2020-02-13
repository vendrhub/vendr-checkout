using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Vendr.Checkout.Web.Controllers
{
    public class VendrCheckoutPageController : RenderMvcController
    {
        public override ActionResult Index(ContentModel model)
        {
            if (model.Content.Children != null)
            {
                var firstChild = model.Content.Children.FirstOrDefault();
                if (firstChild != null)
                {
                    return RedirectPermanent(firstChild.Url);
                }
            }

            return new HttpNotFoundResult();
        }
    }
}