using System;
using System.Web.Http;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using Umbraco.Web.WebApi.Filters;
using Vendr.Checkout.Services;
using Vendr.Core.Api;

namespace Vendr.Checkout.Web.Controllers
{
    [PluginController("VendrCheckout")]
    public class VendrCheckoutApiController : UmbracoAuthorizedApiController
    {
        private IVendrApi _vendrApi;

        public VendrCheckoutApiController(IVendrApi vendrApi)
        {
            _vendrApi = vendrApi;
        }

        [HttpGet]
        [UmbracoApplicationAuthorize(Constants.Applications.Settings)]
        public object InstallVendrCheckout(int siteRootNodeId)
        {
            // Validate the site root node
            var siteRootNode = Services.ContentService.GetById(siteRootNodeId);

            var storeId = GetStoreId(siteRootNode);
            if (!storeId.HasValue)
                return new { success = false, message = "Couldn't find a store connected to the site root node. Do you have a Vendr store picker configured?" };

            var store = _vendrApi.GetStore(storeId.Value);
            if (store == null)
                return new { success = false, message = "Couldn't find a store connected to the site root node. Do you have a Vendr store picker configured?" };

            //  Perform the install
            new InstallService()
                .Install(siteRootNodeId, store);

            // Return success
            return new { success = true };
        }

        private Guid? GetStoreId(IContent content)
        {
            if (content.HasProperty(Core.Constants.Properties.StorePropertyAlias))
                return content.GetValue<Guid?>(Core.Constants.Properties.StorePropertyAlias);

            if (content.ParentId != -1)
                return GetStoreId(Services.ContentService.GetById(content.ParentId));

            return null;
        }
    }
}