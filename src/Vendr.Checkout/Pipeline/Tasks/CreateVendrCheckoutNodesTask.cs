﻿using Vendr.Common.Pipelines;
using Vendr.Common.Pipelines.Tasks;
using System.Reflection;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Cms.Core.Services;

namespace Vendr.Checkout.Pipeline.Tasks
{
    public class CreateVendrCheckoutNodesTask : PipelineTaskBase<InstallPipelineContext>
    {
        private readonly IScopeProvider _scopeProvider;
        private readonly IContentTypeService _contentTypeService;
        private readonly IContentService _contentService;

        public CreateVendrCheckoutNodesTask(IScopeProvider scopeProvider,
            IContentTypeService contentTypeService,
            IContentService contentService)
        {
            _scopeProvider = scopeProvider;
            _contentTypeService = contentTypeService;
            _contentService = contentService;
        }

        public override PipelineResult<InstallPipelineContext> Execute(PipelineArgs<InstallPipelineContext> args)
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                var vendrCheckoutPageContenType = _contentTypeService.Get(VendrCheckoutConstants.ContentTypes.Aliases.CheckoutPage);
                var vendrCheckoutStepPageContenType = _contentTypeService.Get(VendrCheckoutConstants.ContentTypes.Aliases.CheckoutStepPage);

                // Check to see if the checkout node already exists
                var filter = scope.SqlContext.Query<IContent>().Where(x => x.ContentTypeId == vendrCheckoutPageContenType.Id);
                var childNodes = _contentService.GetPagedChildren(args.Model.SiteRootNodeId, 1, 1, out long totalRecords, filter);
                
                if (totalRecords == 0)
                {
                    // Create the checkout page
                    var checkoutNode = _contentService.CreateAndSave("Checkout", args.Model.SiteRootNodeId,
                        VendrCheckoutConstants.ContentTypes.Aliases.CheckoutPage);

                    // Create the checkout steps pages
                    CreateCheckoutStepPage(checkoutNode, "Customer Information", "Information", "Information");
                    CreateCheckoutStepPage(checkoutNode, "Shipping Method", "Shipping Method", "ShippingMethod");
                    CreateCheckoutStepPage(checkoutNode, "Payment Method", "Payment Method", "PaymentMethod");
                    CreateCheckoutStepPage(checkoutNode, "Review Order", "Review", "Review");
                    CreateCheckoutStepPage(checkoutNode, "Process Payment", "Payment", "Payment");
                    CreateCheckoutStepPage(checkoutNode, "Order Confirmation", "Confirmation", "Confirmation");
                }

                scope.Complete();
            }

            // Continue the pipeline
            return Ok();
        }

        private void CreateCheckoutStepPage(IContent parent, string name, string shortName, string stepType)
        {
            var checkoutStepNode = _contentService.Create(name, parent.Id,
                VendrCheckoutConstants.ContentTypes.Aliases.CheckoutStepPage);

            checkoutStepNode.SetValue("vendrShortStepName", shortName);
            checkoutStepNode.SetValue("vendrStepType", $"[\"{stepType}\"]");

            // _contentService.Save(checkoutStepNode);

            // In Umbraco v10 contentService.Save optional parameter userId was switched to a nullable int
            // and so this causes a Method Not Found error and so we have to call it via reflection instead.
            var m = _contentService.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(x => x.Name.Equals("Save", System.StringComparison.InvariantCultureIgnoreCase)
                    && x.GetParameters()[0].ParameterType == typeof(IContent));

            if (m != null)
            {
                var p = m.GetParameters()
                    .Select((x, i) => (object)(i == 0 ? checkoutStepNode : (x.HasDefaultValue ? x.DefaultValue : null)))
                    .ToArray();

                m.Invoke(_contentService, p);
            }
        }
    }
}