using Umbraco.Core.Composing;
using Umbraco.Core.Models;

namespace Vendr.Checkout.Pipeline.Implement.Tasks
{
    internal class CreateVendrCheckoutNodesTask : IPipelineTask<InstallPipelineContext, InstallPipelineContext>
    {
        public InstallPipelineContext Process(InstallPipelineContext ctx)
        {
            using (var scope = Current.ScopeProvider.CreateScope())
            {
                var vendrCheckoutPageContenType = Current.Services.ContentTypeService.Get(VendrCheckoutConstants.ContentTypes.Aliases.CheckoutPage);
                var vendrCheckoutStepPageContenType = Current.Services.ContentTypeService.Get(VendrCheckoutConstants.ContentTypes.Aliases.CheckoutStepPage);

                // Check to see if the checkout node already exists
                var filter = scope.SqlContext.Query<IContent>().Where(x => x.ContentTypeId == vendrCheckoutPageContenType.Id);
                var childNodes = Current.Services.ContentService.GetPagedChildren(ctx.SiteRootNodeId, 1, 1, out long totalRecords, filter);
                
                if (totalRecords == 0)
                {
                    // Create the checkout page
                    var checkoutNode = Current.Services.ContentService.CreateAndSave("Checkout", ctx.SiteRootNodeId,
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
            return ctx;
        }

        private void CreateCheckoutStepPage(IContent parent, string name, string shortName, string stepType)
        {
            var checkoutStepNode = Current.Services.ContentService.Create(name, parent.Id,
                VendrCheckoutConstants.ContentTypes.Aliases.CheckoutStepPage);

            checkoutStepNode.SetValue("vendrShortStepName", shortName);
            checkoutStepNode.SetValue("vendrStepType", $"[\"{stepType}\"]");

            Current.Services.ContentService.Save(checkoutStepNode);
        }
    }
}