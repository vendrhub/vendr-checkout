using System;
using Vendr.Checkout.Services;
using Vendr.Core.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Common.Authorization;

namespace Vendr.Checkout.Web.Controllers
{
    [PluginController("VendrCheckout")]
    public class VendrCheckoutApiController : UmbracoAuthorizedApiController
    {
        private readonly IVendrApi _vendrApi;
        private readonly IContentService _contentService;

        public VendrCheckoutApiController(IVendrApi vendrApi,
            IContentService contentService)
        {
            _vendrApi = vendrApi;
            _contentService = contentService;
        }

        [HttpGet]
        [Authorize(Policy = AuthorizationPolicies.SectionAccessSettings)]
        public object InstallVendrCheckout(GuidUdi siteRootNodeId)
        {
            // Validate the site root node
            var siteRootNode = _contentService.GetById(siteRootNodeId.Guid);

            var storeId = GetStoreId(siteRootNode);
            if (!storeId.HasValue)
                return new { success = false, message = "Couldn't find a store connected to the site root node. Do you have a Vendr store picker configured?" };

            var store = _vendrApi.GetStore(storeId.Value);
            if (store == null)
                return new { success = false, message = "Couldn't find a store connected to the site root node. Do you have a Vendr store picker configured?" };

            //  Perform the install
            new InstallService()
                .Install(siteRootNode.Id, store);

            // Return success
            return new { success = true };
        }

        private Guid? GetStoreId(IContent content)
        {
            if (content.HasProperty(Vendr.Umbraco.Constants.Properties.StorePropertyAlias))
                return content.GetValue<Guid?>(Vendr.Umbraco.Constants.Properties.StorePropertyAlias);

            if (content.ParentId != -1)
                return GetStoreId(_contentService.GetById(content.ParentId));

            return null;
        }
    }
}