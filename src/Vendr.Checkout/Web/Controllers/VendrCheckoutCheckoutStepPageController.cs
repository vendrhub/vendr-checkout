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
    public class VendrCheckoutCheckoutStepPageController : VendrCheckoutBaseController
    {
        private readonly PathHelper _pathHelper;

#if NETFRAMEWORK
        public VendrCheckoutCheckoutStepPageController(PathHelper pathHelper)
        {
            _pathHelper = pathHelper;
        }
#else
        public VendrCheckoutCheckoutStepPageController(ILogger<VendrCheckoutCheckoutStepPageController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor,
            PathHelper pathHelper)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _pathHelper = pathHelper;
        }
#endif

        public override IActionResult Index()
        {
            // Check the cart is valid before continuing 
            if (!IsValidCart(out var redirectUrl))
                return Redirect(redirectUrl);

            // If the page has a template, use it
            if (CurrentPage.TemplateId.HasValue && CurrentPage.TemplateId.Value > 0)
                return base.Index();

            // Get the current step from the page and render the appropriate view
            return View(_pathHelper.GetVendrCheckoutViewPath($"VendrCheckout{CurrentPage.Value<string>("vendrStepType")}Page"), CurrentPage);
        }
    }
}