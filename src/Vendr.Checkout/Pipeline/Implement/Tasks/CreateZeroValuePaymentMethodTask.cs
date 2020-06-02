using Vendr.Core.Api;
using Vendr.Core.Models;

namespace Vendr.Checkout.Pipeline.Implement.Tasks
{
    public class CreateZeroValuePaymentMethodTask : IPipelineTask<InstallPipelineContext, InstallPipelineContext>
    {
        public InstallPipelineContext Process(InstallPipelineContext ctx)
        {
            using (var uow = VendrApi.Instance.Uow.Create())
            {
                if (!VendrApi.Instance.PaymentMethodExists(ctx.Store.Id, VendrCheckoutConstants.PaymentMethods.Aliases.ZeroValue))
                {
                    var paymentMethod = PaymentMethod.Create(uow, ctx.Store.Id, VendrCheckoutConstants.PaymentMethods.Aliases.ZeroValue, "[Vendr Checkout] Zero Value", "zeroValue");

                    paymentMethod.SetSku("VCZV01")
                        .SetTaxClass(ctx.Store.DefaultTaxClassId.Value)
                        .AllowInCountry(ctx.Store.DefaultCountryId.Value);

                    // We need to set the Continue URL to the checkout confirmation page
                    // but we create nodes as unpublished, thus without a URL so we'll
                    // have to listen for the confirmation page being published and
                    // sync it's URL accordingly

                    VendrApi.Instance.SavePaymentMethod(paymentMethod);
                }

                uow.Complete();
            }

            return ctx;
        }
    }
}