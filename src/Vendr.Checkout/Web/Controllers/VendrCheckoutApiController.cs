using System.Web.Http;
using Umbraco.Core;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using Umbraco.Web.WebApi.Filters;
using Vendr.Checkout.Services;

namespace Vendr.Checkout.Web.Controllers
{
    [PluginController("VendrCheckout")]
    public class VendrCheckoutApiController : UmbracoAuthorizedApiController
    {
        [HttpGet]
        [UmbracoApplicationAuthorize(Constants.Applications.Settings)]
        public object InstallVendrCheckout(int rootNodeId)
        {
            var installService = new InstallService();

            installService.Install(rootNodeId);

            return null;
        }
    }
}