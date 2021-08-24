using System.Linq;

#if NETFRAMEWORK
using Umbraco.Web;
using IActionResult = System.Web.Mvc.ActionResult;
#else
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;
#endif

namespace Vendr.Checkout.Web.Controllers
{
    public class VendrCheckoutCheckoutPageController : VendrCheckoutBaseController
    {
#if NET
        public VendrCheckoutCheckoutPageController(ILogger<VendrCheckoutCheckoutPageController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        { }
#endif

        public override IActionResult Index()
        {
            // Check the cart is valid before continuing 
            if (!IsValidCart(out var redirectUrl))
                return Redirect(redirectUrl);

            // If the page has a template, use it
            if (CurrentPage.TemplateId.HasValue && CurrentPage.TemplateId.Value > 0)
                return base.Index();

            // No template so redirect to the first child if one exists
            if (CurrentPage.Children != null)
            {
                var firstChild = CurrentPage.Children.FirstOrDefault();
                if (firstChild != null)
                {
                    return RedirectPermanent(firstChild.Url());
                }
            }

            // Still nothing so 404
            return NotFound();
        }
    }
}