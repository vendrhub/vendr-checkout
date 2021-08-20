using Vendr.Common.Pipelines;
using Vendr.Common.Pipelines.Tasks;
using Vendr.Core.Api;
using Vendr.Core.Models;

namespace Vendr.Checkout.Pipeline.Tasks
{
    public class CreateZeroValuePaymentMethodTask : PipelineTaskBase<InstallPipelineContext>
    {
        private readonly IVendrApi _vendrApi;

        public CreateZeroValuePaymentMethodTask(IVendrApi vendrApi)
        {
            _vendrApi = vendrApi;
        }

        public override PipelineResult<InstallPipelineContext> Execute(PipelineArgs<InstallPipelineContext> args)
        {
            using (var uow = _vendrApi.Uow.Create())
            {
                if (!_vendrApi.PaymentMethodExists(args.Model.Store.Id, VendrCheckoutConstants.PaymentMethods.Aliases.ZeroValue))
                {
                    var paymentMethod = PaymentMethod.Create(uow, args.Model.Store.Id, VendrCheckoutConstants.PaymentMethods.Aliases.ZeroValue, "[Vendr Checkout] Zero Value", "zeroValue");

                    paymentMethod.SetSku("VCZV01")
                        .SetTaxClass(args.Model.Store.DefaultTaxClassId.Value)
                        .AllowInCountry(args.Model.Store.DefaultCountryId.Value);

                    // We need to set the Continue URL to the checkout confirmation page
                    // but we create nodes as unpublished, thus without a URL so we'll
                    // have to listen for the confirmation page being published and
                    // sync it's URL accordingly

                    _vendrApi.SavePaymentMethod(paymentMethod);
                }

                uow.Complete();
            }

            return Ok();
        }
    }
}